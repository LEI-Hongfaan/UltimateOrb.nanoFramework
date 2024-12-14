using System;

namespace System {

    internal static partial class ArrayExtensions {

        static readonly byte[] _Array_EmptyByte = new byte[0];

        public static byte[] Array_EmptyByte() {
            return _Array_EmptyByte;
        }

        static readonly char[] _Array_EmptyChar = new char[0];

        public static char[] Array_EmptyChar() {
            return _Array_EmptyChar;
        }
    }
}
