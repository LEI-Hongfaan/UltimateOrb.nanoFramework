// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

        public static void ObjectDisposedException_ThrowIf(bool condition, object instance) {
            if (condition) {
                ThrowHelper.ThrowObjectDisposedException(instance);
            }
        }
    }
}
