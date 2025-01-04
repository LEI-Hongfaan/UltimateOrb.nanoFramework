// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.nanoFramework {

    /// <summary>
    /// Represents an immutable string of UTF-8 code units.
    /// </summary>
    [Serializable]
    public readonly struct Utf8StringView : System.Collections.IEnumerable {

        readonly string? value__;

        public Utf8StringView(string? value) {
            value__ = value;
        }

        public static implicit operator Utf8StringView(string? value) {
            return new Utf8StringView(value);
        }

        public static implicit operator string?(Utf8StringView value) {
            return value.value__;
        }

        /*
         * STATIC FIELDS
         */
        public static readonly Utf8StringView Empty = "";

        /*
         * OPERATORS
         */
        /// <summary>
        /// Compares two <see cref="Utf8StringView"/> instances for equality using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public static bool operator ==(Utf8StringView left, Utf8StringView right) => Equals(left, right);

        /// <summary>
        /// Compares two <see cref="Utf8StringView"/> instances for inequality using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public static bool operator !=(Utf8StringView left, Utf8StringView right) => !Equals(left, right);

        /*
         * INDEXERS
         */
        [IndexerName("Chars")]
        public extern byte this[int index] {

            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /*
         * METHODS
         */
        /// <summary>
        /// Performs an equality comparison using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public override bool Equals(object? obj) {
            return (obj is Utf8StringView other) && this.Equals(other);
        }

        /// <summary>
        /// Performs an equality comparison using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public bool Equals(Utf8StringView value) {
            var v = value__;
            return v is null ? value.value__ is null : v.Equals(value.value__);
        }

        /// <summary>
        /// Returns a hash code using a <see cref="StringComparison.Ordinal"/> comparison.
        /// </summary>
        public override int GetHashCode() {
            var v = value__;
            return null == v ? 0 : v.GetHashCode();
        }

        /// <summary>
        /// Returns <see langword="true"/> if this UTF-8 text consists of all-ASCII data,
        /// <see langword="false"/> if there is any non-ASCII data within this UTF-8 text.
        /// </summary>
        /// <remarks>
        /// ASCII text is defined as text consisting only of scalar values in the range [ U+0000..U+007F ].
        /// Empty strings are considered to be all-ASCII. The runtime of this method is O(n).
        /// </remarks>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool IsAscii();

        /// <summary>
        /// Returns the entire <see cref="Utf8StringView"/> as an str of UTF-8 bytes.
        /// </summary>
        public byte[]? ToByteArray() {
            var v = value__;
            if (null == v) {
                return null;
            }
            return null == v ? null : ToByteArrayInternal();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern byte[] ToByteArrayInternal();

        /// <summary>
        /// Converts this <see cref="Utf8StringView"/> instance to a <see cref="string"/>.
        /// </summary>
        public override string ToString() {
            return null == value__ ? "" : value__;
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public struct Enumerator : System.Collections.IEnumerator {

            readonly Utf8StringView _string;

            readonly int _length;

            int _index;

            byte _current;

            internal Enumerator(Utf8StringView str) {
                _string = str;
                _length = Utf8SpanByte.GetStringUtf8Length(str.value__);
                _index = 0;
                _current = default;
            }

            public byte Current {

                get => _current;
            }

            object? System.Collections.IEnumerator.Current {

                get => Current;
            }

            public bool MoveNext() {
                var i = _index;
                if (i < _length) {
                    _current = _string[i++];
                    _index = i;
                    return true;
                }
                return false;
            }

            public void Reset() {
                _index = 0;
                _current = default;
            }
        }
    }
}
