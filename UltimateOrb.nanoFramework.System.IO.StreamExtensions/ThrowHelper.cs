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
        internal static void ThrowArgumentNullException(ExceptionArgument argument) {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
            throw GetArgumentOutOfRangeException(argument, resource);
        }

        //[DoesNotReturn]
        internal static void ThrowEndOfFileException() {
            throw CreateEndOfFileException();
        }

        internal static Exception CreateEndOfFileException() =>
            new /*EndOfStream*/IOException(/*SR.*/"IO_EOF_ReadBeyondEOF");

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException_UnreadableStream() {
            throw new NotSupportedException(/*SR.NotSupported_*/"UnreadableStream");
        }

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException_UnwritableStream() {
            throw new NotSupportedException(/*SR.NotSupported_*/"UnwritableStream");
        }

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException_StreamClosed(string? objectName) {
            throw new ObjectDisposedException(/*objectName, *//*SR.ObjectDisposed_*/"StreamClosed");
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        private static string GetArgumentName(ExceptionArgument argument) {
            switch (argument) {
            case ExceptionArgument.count:
                return "count";
            case ExceptionArgument.buffer:
                return "buffer";
            case ExceptionArgument.offset:
                return "offset";
            case ExceptionArgument.minimumBytes:
                return "minimumBytes";
            default:
                //Debug.Fail("The enum value is not defined, please check the ExceptionArgument Enum.");
                Debug.WriteLine("The enum value is not defined, please check the ExceptionArgument Enum.");
                Debug.Assert(false);
                return "";
            }
        }

        private static string GetResourceString(ExceptionResource resource) {
            switch (resource) {
            case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
                return /*SR.ArgumentOutOfRange_*/"NeedNonNegNum";
            case ExceptionResource.ArgumentOutOfRange_NotGreaterThanBufferLength:
                return /*SR.ArgumentOutOfRange_*/"NotGreaterThanBufferLength";
            case ExceptionResource.Argument_InvalidOffLen:
                return /*SR.Argument_*/"InvalidOffLen";
            default:
                //Debug.Fail("The enum value is not defined, please check the ExceptionResource Enum.");
                Debug.WriteLine("The enum value is not defined, please check the ExceptionResource Enum.");
                Debug.Assert(false);
                return "";
            }
        }
    }

    //
    // The convention for this enum is using the argument name as the enum name
    //
    internal enum ExceptionArgument {
        count,
        buffer,
        offset,
        minimumBytes,
    }

    //
    // The convention for this enum is using the resource name as the enum name
    //
    internal enum ExceptionResource {
        ArgumentOutOfRange_NeedNonNegNum,
        ArgumentOutOfRange_NotGreaterThanBufferLength,
        Argument_InvalidOffLen,
    }
}