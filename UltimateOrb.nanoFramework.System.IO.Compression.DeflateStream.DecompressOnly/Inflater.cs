// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.IO.Compression {
    /// <summary>
    /// Provides a wrapper around the ZLib decompression API.
    /// </summary>
    internal sealed class Inflater : IDisposable {
        private const int MinWindowBits = -15;              // WindowBits must be between -8..-15 to ignore the header, 8..15 for
        private const int MaxWindowBits = 47;               // zlib headers, 24..31 for GZip headers, or 40..47 for either Zlib or GZip

        private bool _nonEmptyInput;                        // Whether there is any non empty input
        private bool _finished;                             // Whether the end of the stream has been reached
        private bool _isDisposed;                           // Prevents multiple disposals
        private readonly int _windowBits;                   // The WindowBits parameter passed to Inflater construction
        //private ZLibStreamHandle _zlibStream;    // The handle to the primary underlying zlib stream
        private ZStream _zlibStream;
        //private MemoryHandle _inputBufferHandle;            // The handle to the buffer that provides input to _zlibStream
        private readonly long _uncompressedSize;
        private long _currentInflatedCount;

        private object SyncLock => this;                    // Used to make writing to unmanaged structures atomic

        /// <summary>
        /// Initialized the Inflater with the given windowBits size
        /// </summary>
        internal Inflater(int windowBits, long uncompressedSize = -1) {
            Debug.Assert(windowBits >= MinWindowBits && windowBits <= MaxWindowBits);
            _finished = false;
            _nonEmptyInput = false;
            _isDisposed = false;
            _windowBits = windowBits;
            InflateInit(windowBits);
            _uncompressedSize = uncompressedSize;
        }

        public int AvailableOutput => (int)_zlibStream.avail_out;

        /// <summary>
        /// Returns true if the end of the stream has been reached.
        /// </summary>
        public bool Finished() => _finished;

        readonly byte[] _buffer1 = new byte[1];

        public bool Inflate(out byte b) {
            var bufPtr = _buffer1;
            int bytesRead = InflateVerified(bufPtr, 0, 1);
            Debug.Assert(bytesRead == 0 || bytesRead == 1);
            b = bytesRead == 1 ? bufPtr[0] : default;
                return bytesRead != 0;
        }

        public int Inflate(byte[] bytes, int offset, int length) {
            // If Inflate is called on an invalid or unready inflater, return 0 to indicate no bytes have been read.
            if (length == 0)
                return 0;

            Debug.Assert(null != bytes, "Can't pass in a null output buffer!");
            var bufPtr = bytes;
                return InflateVerified(bufPtr, offset, length);
        }

        public int Inflate(SpanByte destination) {
            // If Inflate is called on an invalid or unready inflater, return 0 to indicate no bytes have been read.
            if (destination.Length == 0)
                return 0;

            return InflateVerified(destination);
        }

        static long Math_Min( long x, long y) {
            return x > y ? y : x;
        }

        public int InflateVerified(byte[] bufPtr, int offset, int length) {
            // State is valid; attempt inflation
            try {
                int bytesRead = 0;
                if (_uncompressedSize == -1) {
                    ReadOutput(bufPtr, offset, length, out bytesRead);
                } else {
                    if (_uncompressedSize > _currentInflatedCount) {
                        length = (int)Math_Min(length, _uncompressedSize - _currentInflatedCount);
                        ReadOutput(bufPtr, offset, length, out bytesRead);
                        _currentInflatedCount += bytesRead;
                    } else {
                        _finished = true;
                        _zlibStream.avail_in = 0;
                    }
                }
                return bytesRead;
            } finally {
                // Before returning, make sure to release input buffer if necessary:
                if (0 == _zlibStream.avail_in && IsInputBufferHandleAllocated) {
                    DeallocateInputBufferHandle();
                }
            }
        }

        public int InflateVerified(SpanByte buffer) {
            var rc = buffer.Length;
            var tc = 0;
            var bc = rc;
            if (bc > 512) {
                bc = 512;
            }
            var b = new byte[bc];
            SpanByte bs = b;
            for (; ; ) {
                if (bc > rc) {
                    bc = rc;
                }
                var c = InflateVerified(b, 0, bc);
                if (c == -1) {
                    return tc;
                }
                bs.CopyTo(buffer.Slice(tc));
                rc -= c;
                tc += c;
                if (rc == 0) {
                    return tc;
                }
            }
        }

        private void ReadOutput(byte[] bufPtr, int offset, int length, out int bytesRead) {
            if (ReadInflateOutput(bufPtr, offset, length, /*FlushCode.NoNoFlush*/zlibConst.NoFlush, out bytesRead) == ErrorCode.StreamEnd) {
                if (!NeedsInput() && IsGzipStream() && IsInputBufferHandleAllocated) {
                    _finished = ResetStreamForLeftoverInput();
                } else {
                    _finished = true;
                }
            }
        }

        const byte GZip_Header_ID1 = 31;
        const byte GZip_Header_ID2 = 139;

        /// <summary>
        /// If this stream has some input leftover that hasn't been processed then we should
        /// check if it is another GZip file concatenated with this one.
        ///
        /// Returns false if the leftover input is another GZip data stream.
        /// </summary>
        private bool ResetStreamForLeftoverInput() {
            Debug.Assert(!NeedsInput());
            Debug.Assert(IsGzipStream());
            Debug.Assert(IsInputBufferHandleAllocated);

            lock (SyncLock) {
                
                var nextInPtr = _zlibStream.next_in;
                uint nextAvailIn = (uint)_zlibStream.avail_in;

                // Check the leftover bytes to see if they start with he gzip header ID bytes
                if (nextInPtr[0] != GZip_Header_ID1 || (nextAvailIn > 1 && nextInPtr[1] != GZip_Header_ID2)) {
                    return true;
                }

                // Trash our existing zstream.
                //_zlibStream.Dispose();

                // Create a new zstream
                InflateInit(_windowBits);

                // SetInput on the new stream to the bits remaining from the last stream
                _zlibStream.next_in = nextInPtr;
                _zlibStream.avail_in = (int)nextAvailIn;
                _finished = false;
            }

            return false;
        }

        internal bool IsGzipStream() => _windowBits >= 24 && _windowBits <= 31;

        public bool NeedsInput() => _zlibStream.avail_in == 0;

        public bool NonEmptyInput() => _nonEmptyInput;
        /*
        public void SetInput(byte[] inputBuffer, int startIndex, int count) {
            Debug.Assert(NeedsInput(), "We have something left in previous input!");
            Debug.Assert(inputBuffer != null);
            Debug.Assert(startIndex >= 0 && count >= 0 && count + startIndex <= inputBuffer.Length);
            Debug.Assert(!IsInputBufferHandleAllocated);

            SetInput(inputBuffer.AsMemory(startIndex, count));
        }
        */
        
        public void SetInput(byte[] inputBuffer, int startIndex, int count) {
            Debug.Assert(NeedsInput(), "We have something left in previous input!");
            Debug.Assert(inputBuffer != null);
            Debug.Assert(startIndex >= 0 && count >= 0 && count + startIndex <= inputBuffer.Length);
            Debug.Assert(!IsInputBufferHandleAllocated);

            if (count == 0)
                return;

            lock (SyncLock) {
                _zlibStream.next_in = inputBuffer;
                _zlibStream.next_in_index = startIndex;
                _zlibStream.avail_in = count;
                _finished = false;
                _nonEmptyInput = true;
            }
        }

        private void Dispose(bool disposing) {
            if (!_isDisposed) {
                if (disposing) {
                    //_zlibStream.Dispose();
                }

                if (IsInputBufferHandleAllocated)
                    DeallocateInputBufferHandle();

                _isDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Inflater() {
            Dispose(false);
        }

        /// <summary>
        /// Creates the ZStream that will handle inflation.
        /// </summary>
        //[MemberNotNull(nameof(_zlibStream))]
        private void InflateInit(int windowBits) {
            ErrorCode error;
            try {
                //error = CreateZLibStreamForInflate(out _zlibStream, windowBits);
                error = (ErrorCode)_zlibStream.inflateInit(windowBits);
            } catch (Exception exception) // could not load the ZLib dll
              {
                throw new /*ZLibException*/IOException(/*SR.*/"ZLibErrorDLLLoadError", exception);
            }

            switch (error) {
            case ErrorCode.Ok:           // Successful initialization
                return;

            case ErrorCode.MemError:     // Not enough memory
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorNotEnoughMemory {0} {1} {2}", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage()));

            case ErrorCode.VersionError: //zlib library is incompatible with the version assumed
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorVersionMismatch {0} {1} {2}", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage()));

            case ErrorCode.StreamError:  // Parameters are invalid
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorIncorrectInitParameters {0} {1} {2}", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage()));

            default:
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorUnexpected {0} {1} {2}", "inflateInit2_", (int)error, _zlibStream.GetErrorMessage()));
            }
        }

        /// <summary>
        /// Wrapper around the ZLib inflate function, configuring the stream appropriately.
        /// </summary>
        private ErrorCode ReadInflateOutput(byte[] bufPtr, int offset, int length, FlushCode flushCode, out int bytesRead) {
            lock (SyncLock) {
                _zlibStream.next_out = bufPtr;
                _zlibStream.next_out_index = offset;
                _zlibStream.avail_out = length;

                ErrorCode errC = Inflate(flushCode);
                bytesRead = length - (int)_zlibStream.avail_out;

                return errC;
            }
        }

        /// <summary>
        /// Wrapper around the ZLib inflate function
        /// </summary>
        private ErrorCode Inflate(FlushCode flushCode) {
            ErrorCode errC;
            try {
                //errC = _zlibStream.Inflate(flushCode);
                errC = (ErrorCode)_zlibStream.inflate((int)flushCode);
            } catch (Exception cause) // could not load the Zlib DLL correctly
              {
                throw new /*ZLibException*/IOException(/*SR.*/"ZLibErrorDLLLoadError", cause);
            }
            switch (errC) {
            case ErrorCode.Ok:           // progress has been made inflating
            case ErrorCode.StreamEnd:    // The end of the input stream has been reached
                return errC;

            case ErrorCode.BufError:     // No room in the output buffer - inflate() can be called again with more space to continue
                return errC;

            case ErrorCode.MemError:     // Not enough memory to complete the operation
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorNotEnoughMemory {0} {1} {2}", "inflate_", (int)errC, _zlibStream.GetErrorMessage()));

            case ErrorCode.DataError:    // The input data was corrupted (input stream not conforming to the zlib format or incorrect check value)
                throw new /*InvalidDataException*/SystemException(/*SR.*/"UnsupportedCompression");

            case ErrorCode.StreamError:  //the stream structure was inconsistent (for example if next_in or next_out was NULL),
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorInconsistentStream {0} {1} {2}", "inflate_", (int)errC, _zlibStream.GetErrorMessage()));

            default:
                throw new /*ZLibException*/IOException(string.Format(/*SR.*/"ZLibErrorUnexpected {0} {1} {2}", "inflate_", (int)errC, _zlibStream.GetErrorMessage()));
            }
        }

        /// <summary>
        /// Frees the GCHandle being used to store the input buffer
        /// </summary>
        private void DeallocateInputBufferHandle() {
            Debug.Assert(IsInputBufferHandleAllocated);

            lock (SyncLock) {
                _zlibStream.avail_in = 0;
                _zlibStream.next_in = null;
                //_zlibStream.next_in_index = default;
                //_inputBufferHandle.Dispose();
            }
        }

        private bool IsInputBufferHandleAllocated => _zlibStream.next_out != null;
    }
}