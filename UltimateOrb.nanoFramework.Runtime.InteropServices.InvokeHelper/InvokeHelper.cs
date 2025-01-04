using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.nanoFramework.Runtime.InteropServices {

    public static partial class InvokeHelper {

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2, int arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2, int arg3, int arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2, int arg3, int arg4, int arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int Invoke(IntPtr func, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8);
    }
}
