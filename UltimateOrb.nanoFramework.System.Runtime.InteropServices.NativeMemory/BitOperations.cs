// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Numerics {

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
    using IntPtr = UltimateOrb.nanoFramework.IntPtr;
    using nuint = UltimateOrb.nanoFramework.nuint;
#endif

    /// <summary>
    /// Utility methods for intrinsic bit-twiddling operations.
    /// The methods use hardware intrinsics when available on the underlying platform,
    /// otherwise they use optimized software fallbacks.
    /// </summary>
    internal static partial class BitOperations {

        /// <summary>
        /// Evaluate whether a given integral value is a power of 2.
        /// </summary>
        /// <param name="value">The value.</param>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        public static bool IsPow2(uint value) => (value & (value - 1)) == 0 && value != 0;

        /// <summary>
        /// Evaluate whether a given integral value is a power of 2.
        /// </summary>
        /// <param name="value">The value.</param>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        public static bool IsPow2(ulong value) => (value & (value - 1)) == 0 && value != 0;

        /// <summary>
        /// Evaluate whether a given integral value is a power of 2.
        /// </summary>
        /// <param name="value">The value.</param>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
#if TARGET_64BIT
        public static bool IsPow2(nuint value) => IsPow2((UInt64)value);
#else
        public static bool IsPow2(nuint value) => IsPow2((UInt32)value);
#endif
    }
}
