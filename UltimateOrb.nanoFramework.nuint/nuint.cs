// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable
using System;

#if TARGET_64BIT
using nint_t = System.Int64;
using nuint_t = System.UInt64;
#else
using nint_t = System.Int32;
using nuint_t = System.UInt32;
#endif

namespace UltimateOrb.nanoFramework {

    public readonly struct nuint : IComparable {

        readonly nuint_t value__;

        public static readonly nuint Zero = default;

        public nuint(UInt32 value) {
            value__ = value;
        }

        public nuint(UInt64 value) {
#if TARGET_64BIT
            value__ = value;
#else
            value__ = checked((UInt32)value);
#endif
        }

        public override bool Equals(object? obj) => (obj is nuint other) && Equals(other);


        public override int GetHashCode() {
#if TARGET_64BIT
            return unchecked((int)((long)value__ ^ (value__ >> 32)));
#else
            return (int)value__;
#endif
        }

        public UInt32 ToUInt32() {
#if TARGET_64BIT
            return checked((UInt32)value__);
#else
            return (UInt32)value__;
#endif
        }

        public UInt64 ToUInt64() => value__;

        public static implicit operator nuint(UInt32 value) => new(value);

        public static explicit operator nuint(UInt64 value) => new(value);

        public static explicit operator UInt32(nuint value) {
#if TARGET_64BIT
            return checked((UInt32)value.value__);
#else
            return (UInt32)value.value__;
#endif
        }

        public static explicit operator UInt64(nuint value) => value.value__;

        public static bool operator ==(nuint value1, nuint value2) => value1.value__ == value2.value__;

        public static bool operator !=(nuint value1, nuint value2) => value1.value__ != value2.value__;

        public static nuint operator +(nuint pouinter, int offset) => new(unchecked((nuint_t)((nint_t)pouinter.value__ + offset)));

        public static nuint operator -(nuint pouinter, int offset) => new(unchecked((nuint_t)((nint_t)pouinter.value__ - offset)));

        public static int Size {

            get => sizeof(nuint_t);
        }

        public static nuint MaxValue {

            get => new(nuint_t.MaxValue);
        }

        public static nuint MinValue {

            get => new(nuint_t.MinValue);
        }

        public int CompareTo(object? value) {
            if (value is nuint other) {
                return CompareTo(other);
            } else if (value is null) {
                return 1;
            }

            throw new ArgumentException(/*SR.*/"Arg_MustBeUIntPtr");
        }

        public int CompareTo(nuint value) {
            if (value__ < value.value__) return -1;
            if (value__ > value.value__) return 1;
            return 0;
        }

        public bool Equals(nuint other) => value__ == other.value__;

        public override string ToString() => ((nuint_t)value__).ToString();

        public string ToString(string? format) => ((nuint_t)value__).ToString(format);

#if INTPTR_FEATURE_EXTENSION1
        public static nuint Parse(string s) => (nuint)nuint_t.Parse(s);

        public static bool TryParse(string? s, out nuint result) {
            var r = nuint_t.TryParse(s, out var t);
            result = (nuint)t;
            return r;
        }
#endif

        //
        // IAdditionOperators
        //

        public static nuint operator +(nuint left, nuint right) => new(unchecked(left.value__ + right.value__));

        // public static nuint operator checked +(nuint left, nuint right) => new(checked(left.value__ + right.value__));

        //
        // IBitwiseOperators
        //

        public static nuint operator &(nuint left, nuint right) => new(left.value__ & right.value__);

        public static nuint operator |(nuint left, nuint right) => new(left.value__ | right.value__);

        public static nuint operator ^(nuint left, nuint right) => new(left.value__ ^ right.value__);

        public static nuint operator ~(nuint value) => new(~value.value__);

        //
        // IComparisonOperators
        //

        public static bool operator <(nuint left, nuint right) => left.value__ < right.value__;

        public static bool operator <=(nuint left, nuint right) => left.value__ <= right.value__;

        public static bool operator >(nuint left, nuint right) => left.value__ > right.value__;

        public static bool operator >=(nuint left, nuint right) => left.value__ >= right.value__;

        //
        // IDecrementOperators
        //

        public static nuint operator --(nuint value) => new(unchecked(value.value__ - 1));

        // public static nuint operator checked --(nuint value) => new(checked(value.value__ - 1));

        //
        // IDivisionOperators
        //

        public static nuint operator /(nuint left, nuint right) => new(left.value__ / right.value__);

        //
        // IIncrementOperators
        //

        public static nuint operator ++(nuint value) => new(unchecked(value.value__ + 1));

        // public static nuint operator checked ++(nuint value) => new(checked(value.value__ + 1));

        //
        // IModulusOperators
        //

        /// <inheritdoc cref="IModulusOperators{TSelf, TOther, TResult}.op_Modulus(TSelf, TOther)" />
        public static nuint operator %(nuint left, nuint right) => new(left.value__ % right.value__);

        //
        // IMultiplyOperators
        //

        public static nuint operator *(nuint left, nuint right) => new(unchecked(left.value__ * right.value__));

        // public static nuint operator checked *(nuint left, nuint right) => new(checked(left.value__ * right.value__));

        //
        // IShiftOperators
        //

        public static nuint operator <<(nuint value, int shiftAmount) => new(value.value__ << shiftAmount);

        public static nuint operator >>(nuint value, int shiftAmount) => new(value.value__ >> shiftAmount);

        // public static nuint operator >>>(nuint value, int shiftAmount) => new(value.value__ >>> shiftAmount);

        //
        // ISubtractionOperators
        //

        public static nuint operator -(nuint left, nuint right) => new(unchecked(left.value__ - right.value__));

        // public static nuint operator checked -(nuint left, nuint right) => new(checked(left.value__ - right.value__));

        //
        // IUnaryNegationOperators
        //

        public static nuint operator -(nuint value) => new(unchecked((nuint_t)0 - value.value__));

        // public static nuint operator checked -(nuint value) => new(checked((nuint_t)0 - value.value__));

        //
        // IUnaryPlusOperators
        //

        public static nuint operator +(nuint value) => value;
    }
}
