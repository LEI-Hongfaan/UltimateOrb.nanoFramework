// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using static System.ExceptionExtensions;
using static System.ArrayExtensions;
#else
using System.Buffers;
using System.Buffers.Binary;
#endif
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;


namespace System.IO
{

    internal static partial class Extensions {

        internal static byte[] Comma(this SpanByte x, byte[] y) {
            return y;
        }
        // We expect this to be the workhorse for NLS Encodings, but for existing
        // ones we need a working (if slow) default implementation)
        //
        // WARNING: If this breaks it could be a security threat.  Obviously we
        // call this internally, so you need to make sure that your pointers, counts
        // and indexes are correct when you call this method.
        //
        // In addition, we have internal code, which will be marked as "safe" calling
        // this code.  However this code is dependent upon the implementation of an
        // external GetChars() method, which could be overridden by a third party and
        // the results of which cannot be guaranteed.  We use that result to copy
        // the char[] to our char* output buffer.  If the result count was wrong, we
        // could easily overflow our output buffer.  Therefore we do an extra test
        // when we copy the buffer so that we don't overflow charCount either.
        [CLSCompliant(false)]
        public static int GetChars(this Decoder decoder, byte[] bytes, int byteCount,
                                           char[] chars, int charCount, bool flush) {
            ArgumentNullException_ThrowIfNull(bytes);
            ArgumentNullException_ThrowIfNull(chars);
            ArgumentOutOfRangeException_ThrowIfNegative(byteCount);
            ArgumentOutOfRangeException_ThrowIfNegative(charCount);

            // Get the byte array to convert
            byte[] arrByte = new byte[byteCount];

            for (int index = 0; index < byteCount; index++)
                arrByte[index] = bytes[index];

            // Get the char array to fill
            char[] arrChar = new char[charCount];

            // Do the work
            //int result = GetChars(arrByte, 0, byteCount, arrChar, 0, flush);
            decoder.Convert(arrByte, 0, byteCount, arrChar, 0, charCount, flush, out _, out var result, out var completed);
            if (!completed) {
                throw new Exception("Encoding decoder error");
            }

            Debug.Assert(result <= charCount, "Returned more chars than we have space for");

            // Copy the char array
            // WARNING: We MUST make sure that we don't copy too many chars.  We can't
            // rely on result because it could be a 3rd party implementation.  We need
            // to make sure we never copy more than charCount chars no matter the value
            // of result
            if (result < charCount)
                charCount = result;

            // We check both result and charCount so that we don't accidentally overrun
            // our pointer buffer just because of an issue in GetChars
            for (int index = 0; index < charCount; index++)
                chars[index] = arrChar[index];

            return charCount;
        }

        public static int GetMaxCharCount(this Encoding encoding, int byteCount) {
            if (encoding is UTF8Encoding t) {
                //ArgumentOutOfRangeException_ThrowIfNegative(byteCount);
                if (0 > byteCount) {
                    throw new ArgumentOutOfRangeException(nameof(byteCount));
                }
                
                // GetMaxCharCount assumes that the caller might have a stateful Decoder instance. If the
                // Decoder instance already has a captured partial UTF-8 subsequence, then one of two
                // thngs will happen:
                //
                // - The next byte(s) won't complete the subsequence but will instead be consumed into
                //   the Decoder's internal state, resulting in no character output; or
                // - The next byte(s) will complete the subsequence, and the previously captured
                //   subsequence and the next byte(s) will result in 1 - 2 chars output; or
                // - The captured subsequence will be treated as a singular ill-formed subsequence, at
                //   which point the captured subsequence will go through the fallback routine.
                //   (See The Unicode Standard, Sec. 3.9 for more information on this.)
                //
                // The third case is the worst-case scenario for expansion, since it means 0 bytes of
                // new input could cause any existing captured state to expand via fallback. So it's
                // what we'll use for any pessimistic "max char count" calculation.

                long charCount = ((long)byteCount + 1); // +1 to account for captured subsequence, as above

                // Non-shortest form would fall back, so get max count from fallback.
                // So would 11... followed by 11..., so you could fall back every byte
                if (/*DecoderFallback.MaxCharCount*/1 > 1) {
                    charCount *= /*DecoderFallback.MaxCharCount*/1;
                }

                if (charCount > 0x7fffffff)
                    throw new ArgumentOutOfRangeException(nameof(byteCount), /*SR.ArgumentOutOfRange_*/"GetCharCountOverflow");

                return (int)charCount;
            }
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Reads primitive data types as binary values in a specific encoding.
    /// </summary>
    public class BinaryReader : IDisposable
    {
        private const int MaxCharBytesSize = 128;

        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private Decoder? _decoder;
        private char[]? _charBuffer;
        private readonly int _maxCharsSize;  // From MaxCharBytesSize & Encoding


        // Performance optimization for Read() w/ Unicode.  Speeds us up by ~40%
        private readonly bool _2BytesPerChar;

        private readonly bool _isMemoryStream; // "do we sit on MemoryStream?" for Read/ReadInt32 perf
        private readonly bool _leaveOpen;
        private bool _disposed;

        public BinaryReader(Stream input) : this(input, Encoding.UTF8, false)
        {
        }

        public BinaryReader(Stream input, Encoding encoding) : this(input, encoding, false)
        {
        }

        public BinaryReader(Stream input, Encoding encoding, bool leaveOpen)
        {
            ArgumentNullException_ThrowIfNull(input);
            ArgumentNullException_ThrowIfNull(encoding);

            if (!input.CanRead)
            {
                throw new ArgumentException(/*SR.Argument_*/"StreamNotReadable");
            }

            _stream = input;
            _encoding = encoding;
            _maxCharsSize = encoding.GetMaxCharCount(MaxCharBytesSize);

            // For Encodings that always use 2 bytes per char (or more),
            // special case them here to make Read() & Peek() faster.
#if NANOFRAMEWORK_1_0
            _2BytesPerChar = false;
#else
            _2BytesPerChar = encoding is UnicodeEncoding;
#endif

            // check if BinaryReader is based on MemoryStream, and keep this for it's life
            // we cannot use "as" operator, since derived classes are not allowed
            _isMemoryStream = _stream.GetType() == typeof(MemoryStream);
            _leaveOpen = leaveOpen;
        }

        public virtual Stream BaseStream => _stream;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && !_leaveOpen)
                {
                    _stream.Close();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <remarks>
        /// Override Dispose(bool) instead of Close(). This API exists for compatibility purposes.
        /// </remarks>
        public virtual void Close()
        {
            Dispose(true);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                ThrowHelper.ThrowObjectDisposedException_FileClosed();
            }
        }

        public virtual int PeekChar()
        {
            ThrowIfDisposed();

            if (!_stream.CanSeek)
            {
                return -1;
            }

            long origPos = _stream.Position;
            int ch = Read();
            _stream.Position = origPos;
            return ch;
        }

        public virtual int Read()
        {
            ThrowIfDisposed();

            int charsRead = 0;
            int numBytes;
            long posSav = 0;

            if (_stream.CanSeek)
            {
                posSav = _stream.Position;
            }

            _decoder ??= _encoding.GetDecoder();
#if NANOFRAMEWORK_1_0
            byte[] charBytes = new byte[MaxCharBytesSize];
#else
            Span<byte> charBytes = stackalloc byte[MaxCharBytesSize];
#endif

            char singleChar = '\0';
            char[] chars = new[] { singleChar };

            while (charsRead == 0)
            {
                // We really want to know what the minimum number of bytes per char
                // is for our encoding.  Otherwise for UnicodeEncoding we'd have to
                // do ~1+log(n) reads to read n characters.
                // Assume 1 byte can be 1 char unless _2BytesPerChar is true.
                numBytes = 1;
                numBytes = _2BytesPerChar ? 2 : 1;

                int r = _stream.ReadByte();
                charBytes[0] = (byte)r;
                if (r == -1)
                {
                    numBytes = 0;
                }
                if (numBytes == 2)
                {
                    r = _stream.ReadByte();
                    charBytes[1] = (byte)r;
                    if (r == -1)
                    {
                        numBytes = 1;
                    }
                }

                if (numBytes == 0)
                {
                    return -1;
                }

                Debug.Assert(numBytes is 1 or 2, "BinaryReader::ReadOneChar assumes it's reading one or two bytes only.");

                try
                {
#if NANOFRAMEWORK_1_0
                    charsRead = _decoder.GetChars(charBytes, numBytes, chars, 1, flush: false);
#else
                    charsRead = _decoder.GetChars(charBytes[..numBytes], new Span<char>(ref singleChar), flush: false);
#endif
                } catch
                {
                    // Handle surrogate char

                    if (_stream.CanSeek)
                    {
                        _stream.Seek(posSav - _stream.Position, SeekOrigin.Current);
                    }
                    // else - we can't do much here

                    throw;
                }

                Debug.Assert(charsRead < 2, "BinaryReader::ReadOneChar - assuming we only got 0 or 1 char, not 2!");
            }
            Debug.Assert(charsRead > 0);
            return singleChar;
        }

        public virtual byte ReadByte() => InternalReadByte();

#if !NANOFRAMEWORK_1_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // Inlined to avoid some method call overhead with InternalRead.
#endif
        private byte InternalReadByte()
        {
            ThrowIfDisposed();

            int b = _stream.ReadByte();
            if (b == -1)
            {
                ThrowHelper.ThrowEndOfFileException();
            }

            return (byte)b;
        }

        [CLSCompliant(false)]
        public virtual sbyte ReadSByte() => (sbyte)InternalReadByte();
        public virtual bool ReadBoolean() => InternalReadByte() != 0;

        public virtual char ReadChar()
        {
            int value = Read();
            if (value == -1)
            {
                ThrowHelper.ThrowEndOfFileException();
            }
            return (char)value;
        }
#if NANOFRAMEWORK_1_0
        static readonly byte[] _buffer = new byte[8];

        public virtual short ReadInt16() => BitConverter.ToInt16(InternalRead(_buffer).Comma(_buffer), 0);

        [CLSCompliant(false)]
        public virtual ushort ReadUInt16() => BitConverter.ToUInt16(InternalRead(_buffer).Comma(_buffer), 0);

        public virtual int ReadInt32() => BitConverter.ToInt32(InternalRead(_buffer).Comma(_buffer), 0);
        [CLSCompliant(false)]
        public virtual uint ReadUInt32() => BitConverter.ToUInt32(InternalRead(_buffer).Comma(_buffer), 0);
        public virtual long ReadInt64() => BitConverter.ToInt64(InternalRead(_buffer).Comma(_buffer), 0);
        [CLSCompliant(false)]
        public virtual ulong ReadUInt64() => BitConverter.ToUInt64(InternalRead(_buffer).Comma(_buffer), 0);

        //public virtual float ReadSingle() => BitConverter.ToSingle(InternalRead(_buffer).Comma(_buffer), 0);
        public virtual double ReadDouble() => BitConverter.ToDouble(InternalRead(_buffer).Comma(_buffer), 0);

#else
        public virtual short ReadInt16() => BinaryPrimitives.ReadInt16LittleEndian(InternalRead(stackalloc byte[sizeof(short)]));

        [CLSCompliant(false)]
        public virtual ushort ReadUInt16() => BinaryPrimitives.ReadUInt16LittleEndian(InternalRead(stackalloc byte[sizeof(ushort)]));

        public virtual int ReadInt32() => BinaryPrimitives.ReadInt32LittleEndian(InternalRead(stackalloc byte[sizeof(int)]));
        [CLSCompliant(false)]
        public virtual uint ReadUInt32() => BinaryPrimitives.ReadUInt32LittleEndian(InternalRead(stackalloc byte[sizeof(uint)]));
        public virtual long ReadInt64() => BinaryPrimitives.ReadInt64LittleEndian(InternalRead(stackalloc byte[sizeof(long)]));
        [CLSCompliant(false)]
        public virtual ulong ReadUInt64() => BinaryPrimitives.ReadUInt64LittleEndian(InternalRead(stackalloc byte[sizeof(ulong)]));
#if !NANOFRAMEWORK_1_0
        public virtual Half ReadHalf() => BinaryPrimitives.ReadHalfLittleEndian(InternalRead(stackalloc byte[sizeof(Half)]));
#endif
        public virtual float ReadSingle() => BinaryPrimitives.ReadSingleLittleEndian(InternalRead(stackalloc byte[sizeof(float)]));
        public virtual double ReadDouble() => BinaryPrimitives.ReadDoubleLittleEndian(InternalRead(stackalloc byte[sizeof(double)]));
#endif

#if !NANOFRAMEWORK_1_0
        public virtual decimal ReadDecimal()
        {
            ReadOnlySpan<byte> span = InternalRead(stackalloc byte[sizeof(decimal)]);
            try
            {
                return decimal.ToDecimal(span);
            }
            catch (ArgumentException e)
            {
                // ReadDecimal cannot leak out ArgumentException
                throw new IOException(/*SR.Arg_*/"DecBitCtor", e);
            }
        }

        public virtual string ReadString()
        {
            ThrowIfDisposed();

            // Length of the string in bytes, not chars
            int stringLength = Read7BitEncodedInt();
            if (stringLength < 0)
            {
                throw new IOException(string.Format(SR.IO_InvalidStringLen_Len, stringLength));
            }

            if (stringLength == 0)
            {
                return string.Empty;
            }

            Span<byte> charBytes = stackalloc byte[MaxCharBytesSize];

            int currPos = 0;
            StringBuilder? sb = null;
            do
            {
                int readLength = Math.Min(MaxCharBytesSize, stringLength - currPos);

                int n = _stream.Read(charBytes[..readLength]);
                if (n == 0)
                {
                    ThrowHelper.ThrowEndOfFileException();
                }

                if (currPos == 0 && n == stringLength)
                {
                    return _encoding.GetString(charBytes[..n]);
                }

                _decoder ??= _encoding.GetDecoder();
                _charBuffer ??= new char[_maxCharsSize];

                int charsRead = _decoder.GetChars(charBytes[..n], _charBuffer, flush: false);

                // Since we could be reading from an untrusted data source, limit the initial size of the
                // StringBuilder instance we're about to get or create. It'll expand automatically as needed.

                sb ??= StringBuilderCache.Acquire(Math.Min(stringLength, StringBuilderCache.MaxBuilderSize)); // Actual string length in chars may be smaller.
                sb.Append(_charBuffer, 0, charsRead);
                currPos += n;
            } while (currPos < stringLength);

            return StringBuilderCache.GetStringAndRelease(sb);
        }
#endif

        public virtual int Read(char[] buffer, int index, int count)
        {
            ArgumentNullException_ThrowIfNull(buffer);

            ArgumentOutOfRangeException_ThrowIfNegative(index);
            ArgumentOutOfRangeException_ThrowIfNegative(count);
            if (buffer.Length - index < count)
            {
                throw new ArgumentException(/*SR.Argument_*/"InvalidOffLen");
            }
            ThrowIfDisposed();
#if NANOFRAMEWORK_1_0
            return InternalReadChars(new SpanChar(buffer, index, count));
#else
            return InternalReadChars(new Span<char>(buffer, index, count));
#endif
        }

#if NANOFRAMEWORK_1_0
        public virtual int Read(SpanChar buffer)
#else
        public virtual int Read(Span<char> buffer)
#endif
        {
            ThrowIfDisposed();
            return InternalReadChars(buffer);
        }

#if NANOFRAMEWORK_1_0
        private int InternalReadChars(SpanChar buffer)
#else
        private int InternalReadChars(Span<char> buffer)
#endif
        {
            Debug.Assert(!_disposed);

            _decoder ??= _encoding.GetDecoder();

            int totalCharsRead = 0;

#if NANOFRAMEWORK_1_0
            byte[] charBytes = new byte[MaxCharBytesSize];

            var buffer2 = new char[buffer.Length];
#else
            Span<byte> charBytes = stackalloc byte[MaxCharBytesSize];
#endif

            while (!buffer.IsEmpty)
            {
                int numBytes = buffer.Length;

                // We really want to know what the minimum number of bytes per char
                // is for our encoding.  Otherwise for UnicodeEncoding we'd have to
                // do ~1+log(n) reads to read n characters.
                if (_2BytesPerChar)
                {
                    numBytes <<= 1;
                }

                // We do not want to read even a single byte more than necessary.
                //
                // Subtract pending bytes that the decoder may be holding onto. This assumes that each
                // decoded char corresponds to one or more bytes. Note that custom encodings or encodings with
                // a custom replacement sequence may violate this assumption.
                if (numBytes > 1)
                {
                    // For internal decoders, we can check whether the decoder has any pending state.
                    // For custom decoders, assume that the decoder has pending state.
#if !NANOFRAMEWORK_1_0
                    if (_decoder is not DecoderNLS decoder || decoder.HasState)
#endif
                    {
                        numBytes--;

                        // The worst case is charsRemaining = 2 and UTF32Decoder holding onto 3 pending bytes. We need to read just
                        // one byte in this case.
                        if (_2BytesPerChar && numBytes > 2)
                            numBytes -= 2;
                    }
                }

#if NANOFRAMEWORK_1_0
                byte[] byteBuffer;
#else
                scoped ReadOnlySpan<byte> byteBuffer;
                if (_isMemoryStream)
                {
                    Debug.Assert(_stream is MemoryStream);
                    MemoryStream mStream = Unsafe.As<MemoryStream>(_stream);

                    int position = mStream.InternalGetPosition();
                    numBytes = mStream.InternalEmulateRead(numBytes);
                    byteBuffer = new ReadOnlySpan<byte>(mStream.InternalGetBuffer(), position, numBytes);
                }
                else
#endif
                {
                    if (numBytes > MaxCharBytesSize)
                    {
                        numBytes = MaxCharBytesSize;
                    }
#if NANOFRAMEWORK_1_0
                    numBytes = _stream.Read(new SpanByte(charBytes, 0, numBytes));
                    byteBuffer = numBytes == 0 ? Array_EmptyByte() : new SpanByte(charBytes, 0, numBytes).ToArray();
#else
                    numBytes = _stream.Read(charBytes[..numBytes]);
                    byteBuffer = charBytes[..numBytes];
#endif
                }

#if NANOFRAMEWORK_1_0
                if (numBytes > 0)
#else
                if (byteBuffer.IsEmpty)
#endif
                {
                    break;
                }

#if NANOFRAMEWORK_1_0
                int charsRead = _decoder.GetChars(byteBuffer, byteBuffer.Length, buffer2, buffer2.Length, flush: false);
                ((SpanChar)buffer2).Slice(charsRead).CopyTo(buffer);
#else
                int charsRead = _decoder.GetChars(byteBuffer, buffer, flush: false);
#endif
                buffer = buffer.Slice(charsRead);

                totalCharsRead += charsRead;
            }

            // we may have read fewer than the number of characters requested if end of stream reached
            // or if the encoding makes the char count too big for the buffer (e.g. fallback sequence)
            return totalCharsRead;
        }

        public virtual char[] ReadChars(int count)
        {
            ArgumentOutOfRangeException_ThrowIfNegative(count);
            ThrowIfDisposed();

            if (count == 0)
            {
#if NANOFRAMEWORK_1_0
                return Array_EmptyChar();
#else
                return Array.Empty<char>();
#endif
            }

            char[] chars = new char[count];
#if NANOFRAMEWORK_1_0
            int n = InternalReadChars(new SpanChar(chars));
#else
            int n = InternalReadChars(new Span<char>(chars));
#endif
            if (n != count)
            {
#if NANOFRAMEWORK_1_0
                chars = new SpanChar(chars, 0, n).ToArray();
#else
                chars = chars[..n];
#endif
            }

            return chars;
        }

        public virtual int Read(byte[] buffer, int index, int count)
        {
            ArgumentNullException_ThrowIfNull(buffer);

            ArgumentOutOfRangeException_ThrowIfNegative(index);
            ArgumentOutOfRangeException_ThrowIfNegative(count);
            if (buffer.Length - index < count)
            {
                throw new ArgumentException(/*SR.Argument_*/"InvalidOffLen");
            }
            ThrowIfDisposed();

            return _stream.Read(buffer, index, count);
        }
#if NANOFRAMEWORK_1_0
        public virtual int Read(SpanByte buffer)
#else
        public virtual int Read(Span<byte> buffer)
#endif
        {
            ThrowIfDisposed();
            return _stream.Read(buffer);
        }

        public virtual byte[] ReadBytes(int count)
        {
            ArgumentOutOfRangeException_ThrowIfNegative(count);
            ThrowIfDisposed();

            if (count == 0)
            {
                return Array_EmptyByte();
            }

            byte[] result = new byte[count];
            int numRead = _stream.ReadAtLeast(result, result.Length, throwOnEndOfStream: false);

            if (numRead != result.Length)
            {
                // Trim array. This should happen on EOF & possibly net streams.
#if NANOFRAMEWORK_1_0
                var r = new byte[numRead];
                Array.Copy(result, 0, r, 0, numRead);
                result = r;
#else
                result = result[..numRead];
#endif
            }

            return result;
        }

        /// <summary>
        /// Reads bytes from the current stream and advances the position within the stream until the <paramref name="buffer" /> is filled.
        /// </summary>
        /// <remarks>
        /// When <paramref name="buffer"/> is empty, this read operation will be completed without waiting for available data in the stream.
        /// </remarks>
        /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current stream.</param>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached before filling the <paramref name="buffer" />.</exception>
#if NANOFRAMEWORK_1_0
        public virtual void ReadExactly(SpanByte buffer)
#else
        public virtual void ReadExactly(Span<byte> buffer)
#endif
        {
            ThrowIfDisposed();
            _stream.ReadExactly(buffer);
        }

#if NANOFRAMEWORK_1_0
        private SpanByte InternalRead(SpanByte buffer)
#else
        private ReadOnlySpan<byte> InternalRead(Span<byte> buffer)
#endif
        {
            Debug.Assert(buffer.Length != 1, "length of 1 should use ReadByte.");

#if !NANOFRAMEWORK_1_0
            if (_isMemoryStream)
            {
                // read directly from MemoryStream buffer
                Debug.Assert(_stream is MemoryStream);
                return Unsafe.As<MemoryStream>(_stream).InternalReadSpan(buffer.Length);
            } else
#endif
            {
                ThrowIfDisposed();

                _stream.ReadExactly(buffer);

                return buffer;
            }
        }

        // FillBuffer is not performing well when reading from MemoryStreams as it is using the public Stream interface.
        // We introduced new function InternalRead which can work directly on the MemoryStream internal buffer or using the public Stream
        // interface when working with all other streams. This function is not needed anymore but we decided not to delete it for compatibility
        // reasons. More about the subject in: https://github.com/dotnet/coreclr/pull/22102
        protected virtual void FillBuffer(int numBytes)
        {
            ArgumentOutOfRangeException_ThrowIfNegative(numBytes);

            ThrowIfDisposed();

            switch (numBytes)
            {
                case 0:
                    // ReadExactly no-ops for empty buffers, so special case numBytes == 0 to preserve existing behavior.
#if NANOFRAMEWORK_1_0
                    int n = _stream.Read(SpanByte.Empty);
#else
                    int n = _stream.Read(Array.Empty<byte>(), 0, 0);
#endif
                    if (n == 0)
                    {
                        ThrowHelper.ThrowEndOfFileException();
                    }
                    break;
                case 1:
                    n = _stream.ReadByte();
                    if (n == -1)
                    {
                        ThrowHelper.ThrowEndOfFileException();
                    }
                    break;
                default:
                    if (_stream.CanSeek)
                    {
                        _stream.Seek(numBytes, SeekOrigin.Current);
                        return;
                    }
#if NANOFRAMEWORK_1_0
                    byte[] buffer = new byte[numBytes];
                    _stream.ReadExactly(buffer);
#else
                    byte[] buffer = ArrayPool<byte>.Shared.Rent(numBytes);
                    _stream.ReadExactly(buffer.AsSpan(0, numBytes));
                    ArrayPool<byte>.Shared.Return(buffer);
#endif
                break;
            }
        }

        public int Read7BitEncodedInt()
        {
            // Unlike writing, we can't delegate to the 64-bit read on
            // 64-bit platforms. The reason for this is that we want to
            // stop consuming bytes if we encounter an integer overflow.

            uint result = 0;
            byte byteReadJustNow;

            // Read the integer 7 bits at a time. The high bit
            // of the byte when on means to continue reading more bytes.
            //
            // There are two failure cases: we've read more than 5 bytes,
            // or the fifth byte is about to cause integer overflow.
            // This means that we can read the first 4 bytes without
            // worrying about integer overflow.

            const int MaxBytesWithoutOverflow = 4;
            for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
            {
                // ReadByte handles end of stream cases for us.
                byteReadJustNow = ReadByte();
                result |= (byteReadJustNow & 0x7Fu) << shift;

                if (byteReadJustNow <= 0x7Fu)
                {
                    return (int)result; // early exit
                }
            }

            // Read the 5th byte. Since we already read 28 bits,
            // the value of this byte must fit within 4 bits (32 - 28),
            // and it must not have the high bit set.

            byteReadJustNow = ReadByte();
            if (byteReadJustNow > 0b_1111u)
            {
                throw new FormatException(/*string.Format_*/"Bad7BitInt");
            }

            result |= (uint)byteReadJustNow << (MaxBytesWithoutOverflow * 7);
            return (int)result;
        }

        public long Read7BitEncodedInt64()
        {
            ulong result = 0;
            byte byteReadJustNow;

            // Read the integer 7 bits at a time. The high bit
            // of the byte when on means to continue reading more bytes.
            //
            // There are two failure cases: we've read more than 10 bytes,
            // or the tenth byte is about to cause integer overflow.
            // This means that we can read the first 9 bytes without
            // worrying about integer overflow.

            const int MaxBytesWithoutOverflow = 9;
            for (int shift = 0; shift < MaxBytesWithoutOverflow * 7; shift += 7)
            {
                // ReadByte handles end of stream cases for us.
                byteReadJustNow = ReadByte();
                result |= (byteReadJustNow & 0x7Ful) << shift;

                if (byteReadJustNow <= 0x7Fu)
                {
                    return (long)result; // early exit
                }
            }

            // Read the 10th byte. Since we already read 63 bits,
            // the value of this byte must fit within 1 bit (64 - 63),
            // and it must not have the high bit set.

            byteReadJustNow = ReadByte();
            if (byteReadJustNow > 0b_1u)
            {
                throw new FormatException(/*string.Format_*/"Bad7BitInt");
            }

            result |= (ulong)byteReadJustNow << (MaxBytesWithoutOverflow * 7);
            return (long)result;
        }
    }
}