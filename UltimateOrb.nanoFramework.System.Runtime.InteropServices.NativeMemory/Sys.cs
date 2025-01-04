// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
using IntPtr = UltimateOrb.nanoFramework.IntPtr;
using nuint = UltimateOrb.nanoFramework.nuint;
#endif

internal static partial class Interop {

    internal static partial class Sys {

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static IntPtr AlignedAlloc(nuint alignment, nuint size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void AlignedFree(IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static IntPtr AlignedRealloc(IntPtr ptr, nuint alignment, nuint new_size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static IntPtr Calloc(nuint num, nuint size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void Free(IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static IntPtr Malloc(nuint size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static IntPtr Realloc(IntPtr ptr, nuint new_size);
    }
}
