// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System {

    internal static partial class ThrowHelper {

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException(object? instance) {
            throw new ObjectDisposedException(instance?.GetType().FullName);
        }
    }
}