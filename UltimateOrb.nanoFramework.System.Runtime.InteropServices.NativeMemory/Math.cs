// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System {

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
    using IntPtr = UltimateOrb.nanoFramework.IntPtr;
    using nuint = UltimateOrb.nanoFramework.nuint;
#endif

    /// <summary>
    /// Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.
    /// </summary>
    internal static partial class Math {

        //[NonVersionable]
        public static nuint Max(nuint val1, nuint val2) {
            return (val1 >= val2) ? val1 : val2;
        }
    }
}
