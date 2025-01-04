#nullable enable
using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.nanoFramework.Runtime.InteropServices {

    public static partial class GCHelper {

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DangerousPin(object? data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DangerousPin(ref byte data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DangerousUnpin(object? data);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DangerousUnpin(ref byte data);
    }
}
