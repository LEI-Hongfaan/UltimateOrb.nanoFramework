// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
    }
}
