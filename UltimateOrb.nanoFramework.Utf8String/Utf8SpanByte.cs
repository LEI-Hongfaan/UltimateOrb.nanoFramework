// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.nanoFramework {

    [Serializable]
    public readonly ref struct Utf8SpanByte {

        private readonly string? _string;
        private readonly int _start;
        private readonly int _length;

        /// <summary>
        /// Creates a new Utf8SpanByte object over the entirety of a specified str.
        /// </summary>
        /// <param name="str">The str from which to create the System.Span object.</param>
        public Utf8SpanByte(string? str) {
            _string = str;
            _start = 0;
            _length = GetStringUtf8Length(str);
        }


        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int GetStringUtf8Length(string? str);

        /// <summary>
        /// Creates a new Utf8SpanByte object that includes a specified number of elements
        /// of an str starting at a specified index.
        /// </summary>
        /// <param name="str">The source str.</param>
        /// <param name="start">The index of the first element to include in the new System.Span</param>
        /// <param name="length">The number of elements to include in the new System.Span</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <para>
        /// str is null, but start or length is non-zero
        /// </para>
        /// <para>-or-</para>
        /// <para>
        /// start is outside the bounds of the str.
        /// </para>
        /// <para>-or-</para>
        /// <para>
        /// <paramref name="start"/> + <paramref name="length"/> exceed the number of elements in the str.
        /// </para>
        /// </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public Utf8SpanByte(string? str, int start, int length) {
            if (str != null) {
                var str_Length = GetStringUtf8Length(str);
                if (start < 0 ||
                    length < 0 ||
                    start + length > str_Length ||
                    start > str_Length) {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                }
            } else if ((start != 0) || (length != 0)) {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
            _string = str;
            _start = start;
            _length = length;
        }

        /// <summary>
        /// Gets the element at the specified zero-based index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is out of range.
        /// </exception>
        public extern byte this[int index] {

            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns an empty System.Span object.
        /// </summary>
        public static Utf8SpanByte Empty => new Utf8SpanByte();

        /// <summary>
        /// Returns the length of the current span.
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// Returns a value that indicates whether the current System.Span is empty.
        /// true if the current span is empty; otherwise, false.
        /// </summary>
        public bool IsEmpty => _length == 0;

        /// <summary>
        /// Copies the contents of this System.Span into a destination System.Span.
        /// </summary>
        /// <param name="destination"> The destination System.Span object.</param>
        /// <exception cref="System.ArgumentException">
        /// destination is shorter than the source <see cref="Utf8SpanByte"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void CopyTo(SpanByte destination);

        /// <summary>
        /// Forms a slice out of the current <see cref="Utf8SpanByte"/> that begins at a specified index.
        /// </summary>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <returns>A span that consists of all elements of the current span from start to the end of the span.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="start"/> is &lt; zero or &gt; <see cref="Length"/>.</exception>
        public Utf8SpanByte Slice(int start) {
            return Slice(start, _length - start);
        }

        /// <summary>
        /// Forms a slice out of the current <see cref="Utf8SpanByte"/> starting at a specified index for a specified length.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice.</param>
        /// <returns>A <see cref="Utf8SpanByte"/> that consists of <paramref name="length"/> number of elements from the current <see cref="Utf8SpanByte"/> starting at <paramref name="start"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="start"/> or <paramref name="start"/> + <paramref name="length"/> is &lt; zero or &gt; <see cref="Length"/>.</exception>
        public Utf8SpanByte Slice(int start, int length) {
            if ((start < 0) || (length < 0) || (start + length > _length)) {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }

            return new Utf8SpanByte(_string, _start + start, length);
        }


        static readonly byte[] Array_Empty_byte_ = new byte[0];

        /// <summary>
        /// Copies the contents of this <see cref="Utf8SpanByte"/> into a new str.
        /// </summary>
        /// <returns> An str containing the data in the current span.</returns>
        public byte[] ToArray() {
            var c = _length;
            if (c == 0) {
                return Array_Empty_byte_;
            }
            return ToArrayInternal();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern byte[] ToArrayInternal();

        /// <summary>
        /// Implicit conversion of an str to a <see cref="Utf8SpanByte"/>.
        /// </summary>
        /// <param name="array"></param>
        public static implicit operator Utf8SpanByte(string array) {
            return new Utf8SpanByte(array);
        }
    }
}
