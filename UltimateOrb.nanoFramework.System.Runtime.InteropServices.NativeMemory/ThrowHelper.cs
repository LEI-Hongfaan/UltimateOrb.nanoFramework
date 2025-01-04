// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace System {

    internal static partial class ThrowHelper {

        //[DoesNotReturn]
        internal static void ThrowOutOfMemoryException() {
            throw new OutOfMemoryException();
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException(ExceptionResource resource) {
            throw GetArgumentException(resource);
        }

        private static ArgumentException GetArgumentException(ExceptionResource resource) {
            return new ArgumentException(GetResourceString(resource));
        }

        //
        // The convention for this enum is using the resource name as the enum name
        //
        internal enum ExceptionResource {
            Argument_AlignmentMustBePow2,
        }

        private static string GetResourceString(ExceptionResource resource) {
            switch (resource) {
            case ExceptionResource.Argument_AlignmentMustBePow2:
                return /*SR.Argument_*/"AlignmentMustBePow2";
            default:
                //Debug.Fail("The enum value is not defined, please check the ExceptionResource Enum.");
                Debug.WriteLine("The enum value is not defined, please check the ExceptionResource Enum.");
                Debug.Assert(false);
                return "";
            }
        }
    }
}