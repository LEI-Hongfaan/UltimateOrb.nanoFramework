// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable
using System;

#if TARGET_64BIT
using nint_t = System.Int64;
#else
using nint_t = System.Int32;
#endif

namespace UltimateOrb.nanoFramework {

    public readonly struct IntPtr : IComparable {

#if TARGET_64BIT
        readonly Int64 value__;
#else
        readonly Int32 value__;
#endif

        public static readonly IntPtr Zero = new IntPtr(0);

        public IntPtr(Int32 value) {
            value__ = value;
        }

        public IntPtr(Int64 value) {
#if TARGET_64BIT
            value__ = value;
#else
            value__ = checked((Int32)value);
#endif
        }

        public override bool Equals(object? obj) => (obj is IntPtr other) && Equals(other);


        public override int GetHashCode() {
#if TARGET_64BIT
            return unchecked((int)((long)value__ ^ (value__ >> 32)));
#else
            return (int)value__;
#endif
        }

        public Int32 ToInt32() {
#if TARGET_64BIT
            return checked((Int32)value__);
#else
            return (Int32)value__;
#endif
        }

        public Int64 ToInt64() => value__;

        public static explicit operator IntPtr(Int32 value) => new(value);

        public static explicit operator IntPtr(Int64 value) => new(value);

        public static explicit operator Int32(IntPtr value) {
#if TARGET_64BIT
            return checked((Int32)value.value__);
#else
            return (Int32)value.value__;
#endif
        }

        public static explicit operator Int64(IntPtr value) => value.value__;

        public static bool operator ==(IntPtr value1, IntPtr value2) => value1.Equals(value2.value__);

        public static bool operator !=(IntPtr value1, IntPtr value2) => !value1.Equals(value2.value__);

        public static IntPtr operator +(IntPtr pointer, int offset) => new(unchecked(pointer.value__ + offset));

        public static IntPtr operator -(IntPtr pointer, int offset) => new(unchecked(pointer.value__ - offset));

        public static int Size {

            get => sizeof(nint_t);
        }

#if INTPTR_FEATURE_EXTENSION1
        public static IntPtr MaxValue {

            get => new(nint_t.MaxValue);
        }

        public static IntPtr MinValue {

            get => new(nint_t.MinValue);
        }
#endif

        public int CompareTo(object? value) {
            if (value is IntPtr other) {
                return CompareTo(other);
            } else if (value is null) {
                return 1;
            }

            throw new ArgumentException(/*SR.*/"Arg_MustBeIntPtr");
        }

        public int CompareTo(IntPtr value) {
            if (value__ < value.value__) return -1;
            if (value__ > value.value__) return 1;
            return 0;
        }

        public bool Equals(IntPtr other) => value__ == other.value__;

        public override string ToString() => ((nint_t)value__).ToString();

        public string ToString(string? format) => ((nint_t)value__).ToString(format);

#if INTPTR_FEATURE_EXTENSION1
        public static IntPtr Parse(string s) => (IntPtr)nint_t.Parse(s);

        public static bool TryParse(string? s, out IntPtr result) {
            var r = nint_t.TryParse(s, out var t);
            result = (IntPtr)t;
            return r;
        }
#endif
    }
}
