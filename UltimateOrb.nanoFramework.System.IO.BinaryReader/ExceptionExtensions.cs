using System;
using System.Text;

namespace System {

    internal static partial class ExceptionExtensions {

        public static void ArgumentNullException_ThrowIfNull(object? arg) {
            if (arg == null) throw new ArgumentNullException(nameof(arg));
        }

        public static void ArgumentOutOfRangeException_ThrowIfNegative(int arg) {
            if (arg < 0) throw new ArgumentOutOfRangeException(nameof(arg), "NeedNonNegNum");
        }
    }
}
