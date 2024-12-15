// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using static System.ExceptionExtensions;

[assembly: InternalsVisibleTo("UltimateOrb.nanoFramework.System.IO.Compression.DeflateStream.DecompressOnly")]
[assembly: InternalsVisibleTo("UltimateOrb.nanoFramework.System.IO.Compression.DeflateStream")]

namespace System.IO {

    public static partial class StreamExtensions {

        /// <summary>
        /// Reads bytes from the current stream and advances the position within the stream until the <paramref name="buffer"/> is filled.
        /// </summary>
        /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current stream.</param>
        /// <exception cref="EndOfStreamException">
        /// The end of the stream is reached before filling the <paramref name="buffer"/>.
        /// </exception>
        /// <remarks>
        /// When <paramref name="buffer"/> is empty, this read operation will be completed without waiting for available data in the stream.
        /// </remarks>
        public static void ReadExactly(this Stream stream, SpanByte buffer) {
            _ = stream.ReadAtLeastCore(buffer, buffer.Length, throwOnEndOfStream: true);
        }

        public static void ReadExactly(this Stream stream, byte[] buffer, int offset, int count) {
            stream.ReadExactly(new SpanByte(buffer, offset, count));
        }

        /// <summary>
        /// Reads at least a minimum number of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current stream.</param>
        /// <param name="minimumBytes">The minimum number of bytes to read into the buffer.</param>
        /// <param name="throwOnEndOfStream">
        /// <see langword="true"/> to throw an exception if the end of the stream is reached before reading <paramref name="minimumBytes"/> of bytes;
        /// <see langword="false"/> to return less than <paramref name="minimumBytes"/> when the end of the stream is reached.
        /// The default is <see langword="true"/>.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer. This is guaranteed to be greater than or equal to <paramref name="minimumBytes"/>
        /// when <paramref name="throwOnEndOfStream"/> is <see langword="true"/>. This will be less than <paramref name="minimumBytes"/> when the
        /// end of the stream is reached and <paramref name="throwOnEndOfStream"/> is <see langword="false"/>. This can be less than the number
        /// of bytes allocated in the buffer if that many bytes are not currently available.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minimumBytes"/> is negative, or is greater than the length of <paramref name="buffer"/>.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// <paramref name="throwOnEndOfStream"/> is <see langword="true"/> and the end of the stream is reached before reading
        /// <paramref name="minimumBytes"/> bytes of data.
        /// </exception>
        /// <remarks>
        /// When <paramref name="minimumBytes"/> is 0 (zero), this read operation will be completed without waiting for available data in the stream.
        /// </remarks>
        public static int ReadAtLeast(this Stream stream, SpanByte buffer, int minimumBytes, bool throwOnEndOfStream = true) {
            ValidateReadAtLeastArguments(buffer.Length, minimumBytes);

            return stream.ReadAtLeastCore(buffer, minimumBytes, throwOnEndOfStream);
        }

        // No argument checking is done here. It is up to the caller.
        private static int ReadAtLeastCore(this Stream stream, SpanByte buffer, int minimumBytes, bool throwOnEndOfStream) {
            Debug.Assert(minimumBytes <= buffer.Length);

            int totalRead = 0;
            while (totalRead < minimumBytes) {
                int read = stream.Read(buffer.Slice(totalRead));
                if (read == 0) {
                    if (throwOnEndOfStream) {
                        ThrowHelper.ThrowEndOfFileException();
                    }

                    return totalRead;
                }

                totalRead += read;
            }

            return totalRead;
        }

        private static void ValidateReadAtLeastArguments(int bufferLength, int minimumBytes) {
            if (minimumBytes < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.minimumBytes, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (bufferLength < minimumBytes) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.minimumBytes, ExceptionResource.ArgumentOutOfRange_NotGreaterThanBufferLength);
            }
        }

        /// <summary>Validates arguments provided to the <see cref="CopyTo(Stream, int)"/> or <see cref="CopyToAsync(Stream, int, CancellationToken)"/> methods.</summary>
        /// <param name="destination">The <see cref="Stream"/> "destination" argument passed to the copy method.</param>
        /// <param name="bufferSize">The integer "bufferSize" argument passed to the copy method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="destination"/> was null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> was not a positive value.</exception>
        /// <exception cref="NotSupportedException"><paramref name="destination"/> does not support writing.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="destination"/> does not support writing or reading.</exception>
        internal static void ValidateCopyToArguments(Stream destination, int bufferSize) {
            ArgumentNullException_ThrowIfNull(destination, nameof(destination));

            ArgumentOutOfRangeException_ThrowIfNegativeOrZero(bufferSize, nameof(bufferSize));

            if (!destination.CanWrite) {
                if (destination.CanRead) {
                    ThrowHelper.ThrowNotSupportedException_UnwritableStream();
                }

                ThrowHelper.ThrowObjectDisposedException_StreamClosed(destination.GetType().Name);
            }
        }

        public static void CopyTo(this Stream stream, Stream destination, int bufferSize) {
            ValidateCopyToArguments(destination, bufferSize);
            if (!stream.CanRead) {
                if (stream.CanWrite) {
                    ThrowHelper.ThrowNotSupportedException_UnreadableStream();
                }

                ThrowHelper.ThrowObjectDisposedException_StreamClosed(stream.GetType().Name);
            }

            //byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            byte[] buffer = new byte[bufferSize];
            try {
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0) {
                    destination.Write(buffer, 0, bytesRead);
                }
            } finally {
                //ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        /// <summary>Validates arguments provided to reading and writing methods on <see cref="Stream"/>.</summary>
        /// <param name="buffer">The array "buffer" argument passed to the reading or writing method.</param>
        /// <param name="offset">The integer "offset" argument passed to the reading or writing method.</param>
        /// <param name="count">The integer "count" argument passed to the reading or writing method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> was null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> was outside the bounds of <paramref name="buffer"/>, or
        /// <paramref name="count"/> was negative, or the range specified by the combination of
        /// <paramref name="offset"/> and <paramref name="count"/> exceed the length of <paramref name="buffer"/>.
        /// </exception>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ValidateBufferArguments(byte[] buffer, int offset, int count) {
            if (buffer is null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffer);
            }

            if (offset < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if ((uint)count > buffer.Length - offset) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.Argument_InvalidOffLen);
            }
        }
    }
}
