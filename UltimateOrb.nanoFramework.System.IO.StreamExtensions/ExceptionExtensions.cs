// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;

namespace System {

    internal static partial class ExceptionExtensions {

        public static void ArgumentNullException_ThrowIfNull(object? argument, string paramName = "argument") {
            if (argument == null) throw new ArgumentNullException(paramName);
        }

        public static void ArgumentOutOfRangeException_ThrowIfNegative(int value, string paramName = "value") {
            if (value < 0) throw new ArgumentOutOfRangeException(paramName/*, value*/, string.Format(/*SR.ArgumentOutOfRange_Generic_*/"MustBeNonNegative {0} {1}", paramName, value));
        }

        public static void ArgumentOutOfRangeException_ThrowIfNegativeOrZero(int value, string paramName = "value") {
            if (value <= 0) throw new ArgumentOutOfRangeException(paramName/*, value*/, string.Format(/*SR.ArgumentOutOfRange_Generic_*/"MustBeNonNegativeNonZero {0} {1}", paramName, value));
        }
    }
}
