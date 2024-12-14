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
        internal static void ThrowInvalidTypeWithPointersNotSupported(Type targetType) {
            throw new ArgumentException(string.Format(/*SR.Argument_*/"InvalidTypeWithPointersNotSupported {0}", targetType));
        }

        //[DoesNotReturn]
        internal static void ThrowIndexOutOfRangeException() {
            throw new IndexOutOfRangeException();
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException() {
            throw new ArgumentOutOfRangeException();
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_DestinationTooShort() {
            throw new ArgumentException(/*SR.Argument_*/"DestinationTooShort", "destination");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_InvalidTimeSpanStyles() {
            throw new ArgumentException(/*SR.Argument_*/"InvalidTimeSpanStyles", "styles");
        }
        
        ////[DoesNotReturn]
        //internal static void ThrowArgumentException_InvalidEnumValue<TEnum>(TEnum value, [CallerArgumentExpression(nameof(value))] string argumentName = "") {
        //    throw new ArgumentException(string.Format(/*SR.Argument_*/"InvalidEnumValue", value, typeof(TEnum).Name), argumentName);
        //}

        //[DoesNotReturn]
        internal static void ThrowArgumentException_OverlapAlignmentMismatch() {
            throw new ArgumentException(/*SR.Argument_*/"OverlapAlignmentMismatch");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_ArgumentNull_TypedRefType() {
            throw new ArgumentNullException("value", /*SR.ArgumentNull_*/"TypedRefType");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_CannotExtractScalar(ExceptionArgument argument) {
            throw GetArgumentException(ExceptionResource.Argument_CannotExtractScalar, argument);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_TupleIncorrectType(object obj) {
            throw new ArgumentException(string.Format(/*SR.ArgumentException_*/"ValueTupleIncorrectType", obj.GetType()), "other");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_IndexMustBeLessException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLess);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLessOrEqual);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_BadComparer(object? comparer) {
            throw new ArgumentException(string.Format(/*SR.Arg_*/"BogusIComparer", comparer));
        }

        //[DoesNotReturn]
        internal static void ThrowIndexArgumentOutOfRange_NeedNonNegNumException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index,
                                                    ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        }

        //[DoesNotReturn]
        internal static void ThrowValueArgumentOutOfRange_NeedNonNegNumException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.value,
                                                    ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        }

        //[DoesNotReturn]
        internal static void ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.length,
                                                    ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        }

        //[DoesNotReturn]
        internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLessOrEqual() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.startIndex,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLessOrEqual);
        }

        //[DoesNotReturn]
        internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.startIndex,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLess);
        }

        //[DoesNotReturn]
        internal static void ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.count,
                                                    ExceptionResource.ArgumentOutOfRange_Count);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_Year() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.year,
                                                    ExceptionResource.ArgumentOutOfRange_Year);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_Month(int month) {
            throw new ArgumentOutOfRangeException(nameof(month)/*, month*/, /*SR.ArgumentOutOfRange_*/"Month");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_DayNumber(int dayNumber) {
            throw new ArgumentOutOfRangeException(nameof(dayNumber)/*, dayNumber*/, /*SR.ArgumentOutOfRange_*/"DayNumber");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_BadYearMonthDay() {
            throw new ArgumentOutOfRangeException(null, /*SR.ArgumentOutOfRange_*/"BadYearMonthDay");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_BadHourMinuteSecond() {
            throw new ArgumentOutOfRangeException(null, /*SR.ArgumentOutOfRange_*/"BadHourMinuteSecond");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_TimeSpanTooLong() {
            throw new ArgumentOutOfRangeException(null, /*SR.Overflow_*/"TimeSpanTooLong");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_RoundingDigits(string name) {
            throw new ArgumentOutOfRangeException(name, /*SR.ArgumentOutOfRange_*/"RoundingDigits");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_RoundingDigits_MathF(string name) {
            throw new ArgumentOutOfRangeException(name, /*SR.ArgumentOutOfRange_*/"RoundingDigits_MathF");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_Range<T>(string parameterName, T value, T minInclusive, T maxInclusive) {
            throw new ArgumentOutOfRangeException(parameterName/*, value*/, string.Format(/*SR.ArgumentOutOfRange_*/"Range", minInclusive, maxInclusive));
        }
        /*
        //[DoesNotReturn]
        internal static void ThrowOverflowException() {
            throw new OverflowException();
        }

        //[DoesNotReturn]
        internal static void ThrowOverflowException_NegateTwosCompNum() {
            throw new OverflowException(SR.Overflow_NegateTwosCompNum);
        }

        //[DoesNotReturn]
        internal static void ThrowOverflowException_TimeSpanTooLong() {
            throw new OverflowException(SR.Overflow_TimeSpanTooLong);
        }

        //[DoesNotReturn]
        internal static void ThrowOverflowException_TimeSpanDuration() {
            throw new OverflowException(SR.Overflow_Duration);
        }
        */
        //[DoesNotReturn]
        internal static void ThrowArgumentException_Arg_CannotBeNaN() {
            throw new ArgumentException(/*SR.Arg_*/"CannotBeNaN");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_Arg_CannotBeNaN(ExceptionArgument argument) {
            throw new ArgumentException(/*SR.Arg_*/"CannotBeNaN", GetArgumentName(argument));
        }

        /*
        //[DoesNotReturn]
        internal static void ThrowWrongKeyTypeArgumentException<T>(T key, Type targetType) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetWrongKeyTypeArgumentException((object?)key, targetType);
        }

        //[DoesNotReturn]
        internal static void ThrowWrongValueTypeArgumentException<T>(T value, Type targetType) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetWrongValueTypeArgumentException((object?)value, targetType);
        }
        */

        //[DoesNotReturn]
        internal static void ThrowWrongKeyTypeArgumentException(object? key, Type targetType) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetWrongKeyTypeArgumentException((object?)key, targetType);
        }

        //[DoesNotReturn]
        internal static void ThrowWrongValueTypeArgumentException(object? value, Type targetType) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetWrongValueTypeArgumentException((object?)value, targetType);
        }

        private static ArgumentException GetAddingDuplicateWithKeyArgumentException(object? key) {
            return new ArgumentException(string.Format(/*SR.Argument_*/"AddingDuplicateWithKey", key));
        }

        /*
        //[DoesNotReturn]
        internal static void ThrowAddingDuplicateWithKeyArgumentException<T>(T key) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetAddingDuplicateWithKeyArgumentException((object?)key);
        }

        //[DoesNotReturn]
        internal static void ThrowKeyNotFoundException<T>(T key) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetKeyNotFoundException((object?)key);
        }
        */

        //[DoesNotReturn]
        internal static void ThrowAddingDuplicateWithKeyArgumentException(object? key) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetAddingDuplicateWithKeyArgumentException((object?)key);
        }

        //[DoesNotReturn]
        internal static void ThrowKeyNotFoundException(object? key) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetKeyNotFoundException((object?)key);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException(ExceptionResource resource) {
            throw GetArgumentException(resource);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument) {
            throw GetArgumentException(resource, argument);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_HandleNotSync(string paramName) {
            throw new ArgumentException(/*SR.Arg_*/"HandleNotSync", paramName);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_HandleNotAsync(string paramName) {
            throw new ArgumentException(/*SR.Arg_*/"HandleNotAsync", paramName);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument) {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionResource resource) {
            throw new ArgumentNullException(GetResourceString(resource));
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument, ExceptionResource resource) {
            throw new ArgumentNullException(GetArgumentName(argument), GetResourceString(resource));
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument) {
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
            throw GetArgumentOutOfRangeException(argument, resource);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, int paramNumber, ExceptionResource resource) {
            throw GetArgumentOutOfRangeException(argument, paramNumber, resource);
        }

        //[DoesNotReturn]
        internal static void ThrowEndOfFileException() {
            throw CreateEndOfFileException();
        }

        internal static Exception CreateEndOfFileException() =>
            new /*EndOfStream*/IOException(/*SR.*/"IO_EOF_ReadBeyondEOF");
        
        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException() {
            throw new InvalidOperationException();
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException(ExceptionResource resource) {
            throw GetInvalidOperationException(resource);
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException(ExceptionResource resource, Exception e) {
            throw new InvalidOperationException(GetResourceString(resource), e);
        }

        //[DoesNotReturn]
        internal static void ThrowNullReferenceException() {
            throw new NullReferenceException(/*SR.Arg_*/"NullArgumentNullRef");
        }

        ////[DoesNotReturn]
        //internal static void ThrowSerializationException(ExceptionResource resource) {
        //    throw new SerializationException(GetResourceString(resource));
        //}

        ////[DoesNotReturn]
        //internal static void ThrowRankException(ExceptionResource resource) {
        //    throw new RankException(GetResourceString(resource));
        //}

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException(ExceptionResource resource) {
            throw new NotSupportedException(GetResourceString(resource));
        }

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException_UnseekableStream() {
            throw new NotSupportedException(/*SR.NotSupported_*/"UnseekableStream");
        }

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException_UnreadableStream() {
            throw new NotSupportedException(/*SR.NotSupported_*/"UnreadableStream");
        }

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException_UnwritableStream() {
            throw new NotSupportedException(/*SR.NotSupported_*/"UnwritableStream");
        }

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException(object? instance) {
            throw new ObjectDisposedException(instance?.GetType().FullName);
        }

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException(Type? type) {
            throw new ObjectDisposedException(type?.FullName);
        }

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException_StreamClosed(string? objectName) {
            throw new ObjectDisposedException(/*objectName, *//*SR.ObjectDisposed_*/"StreamClosed");
        }

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException_FileClosed() {
            throw new ObjectDisposedException(/*null, *//*SR.ObjectDisposed_*/"FileClosed");
        }

        //[DoesNotReturn]
        internal static void ThrowObjectDisposedException(ExceptionResource resource) {
            throw new ObjectDisposedException(/*null, */GetResourceString(resource));
        }

        //[DoesNotReturn]
        internal static void ThrowNotSupportedException() {
            throw new NotSupportedException();
        }

        /*
        //[DoesNotReturn]
        internal static void ThrowAggregateException(List<Exception> exceptions) {
            throw new AggregateException(exceptions);
        }
        */

        //[DoesNotReturn]
        internal static void ThrowOutOfMemoryException() {
            throw new OutOfMemoryException();
        }

        //[DoesNotReturn]
        internal static void ThrowDivideByZeroException() {
            //throw new DivideByZeroException();
            var a = 1;
            _ = a / 0;
            throw null;
        }

        //[DoesNotReturn]
        internal static void ThrowOutOfMemoryException_StringTooLong() {
            throw new OutOfMemoryException(/*SR.OutOfMemory_*/"StringTooLong");
        }

        //[DoesNotReturn]
        internal static void ThrowOutOfMemoryException_LockEnter_WaiterCountOverflow() {
            throw new OutOfMemoryException(/*SR.*/"Lock_Enter_WaiterCountOverflow"/*_OutOfMemoryException*/);
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_Argument_IncompatibleArrayType() {
            throw new ArgumentException(/*SR.Argument_*/"IncompatibleArrayType");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentException_InvalidHandle(string? paramName) {
            throw new ArgumentException(/*SR.Arg_*/"InvalidHandle", paramName);
        }

        //[DoesNotReturn]
        internal static void ThrowUnexpectedStateForKnownCallback(object? state) {
            throw new ArgumentOutOfRangeException(nameof(state)/*, state*/, /*SR.Argument_*/"UnexpectedStateForKnownCallback");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"EnumNotStarted");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"EnumEnded");
        }

        /*
        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_EnumCurrent(int index) {
            throw GetInvalidOperationException_EnumCurrent(index);
        }
        */

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"EnumFailedVersion");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"EnumOpCantHappen");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_NoValue() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"NoValue");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_ConcurrentOperationsNotSupported() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"ConcurrentOperationsNotSupported");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_HandleIsNotInitialized() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"HandleIsNotInitialized");
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_HandleIsNotPinned() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"HandleIsNotPinned");
        }

        //[DoesNotReturn]
        internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array? array, int offset, int count) {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
        }

        //[DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidUtf8() {
            throw new InvalidOperationException(/*SR.InvalidOperation_*/"InvalidUtf8");
        }

        //[DoesNotReturn]
        internal static void ThrowFormatException_BadFormatSpecifier() {
            throw new FormatException(/*SR.Argument_*/"BadFormatSpecifier");
        }

        //[DoesNotReturn]
        internal static void ThrowFormatException_NeedSingleChar() {
            throw new FormatException(/*SR.Format_*/"NeedSingleChar");
        }

        ////[DoesNotReturn]
        //internal static void ThrowFormatException_BadBoolean(SpanChar value) {
        //    throw new FormatException(/*SR.*/string.Format(/*string.Format_*/"BadBoolean", new string(value)));
        //}

        ////[DoesNotReturn]
        //internal static void ThrowArgumentOutOfRangeException_PrecisionTooLarge() {
        //    throw new ArgumentOutOfRangeException("precision", string.Format(/*SR.Argument_*/"PrecisionTooLarge", StandardFormat.MaxPrecision));
        //}

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException_SymbolDoesNotFit() {
            throw new ArgumentOutOfRangeException("symbol", /*SR.Argument_*/"BadFormatSpecifier");
        }

        //[DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException_NeedNonNegNum(string paramName) {
            throw new ArgumentOutOfRangeException(paramName, /*SR.ArgumentOutOfRange_*/"NeedNonNegNum");
        }

        //[DoesNotReturn]
        internal static void ArgumentOutOfRangeException_Enum_Value() {
            throw new ArgumentOutOfRangeException("value", /*SR.ArgumentOutOfRange_*/"Enum");
        }

        /*
        //[DoesNotReturn]
        internal static void ThrowApplicationException(int hr) {
            // Get a message for this HR
            Exception? ex = Marshal.GetExceptionForHR(hr);
            if (ex != null && !string.IsNullOrEmpty(ex.Message)) {
                ex = new ApplicationException(ex.Message);
            } else {
                ex = new ApplicationException();
            }

            ex.HResult = hr;
            throw ex;
        }
        */

        //[DoesNotReturn]
        internal static void ThrowFormatInvalidString() {
            throw new FormatException(/*SR.Format_*/"InvalidString");
        }

        //[DoesNotReturn]
        internal static void ThrowFormatInvalidString(int offset, ExceptionResource resource) {
            throw new FormatException(string.Format(/*SR.Format_*/"InvalidStringWithOffsetAndReason", offset, GetResourceString(resource)));
        }

        //[DoesNotReturn]
        internal static void ThrowFormatIndexOutOfRange() {
            throw new FormatException(/*SR.Format_*/"IndexOutOfRange");
        }
        /*
        //[DoesNotReturn]
        internal static void ThrowSynchronizationLockException_LockExit() {
            throw new SynchronizationLockException(SR.Lock_Exit_SynchronizationLockException);
        }

        internal static AmbiguousMatchException GetAmbiguousMatchException(MemberInfo memberInfo) {
            Type? declaringType = memberInfo.DeclaringType;
            return new AmbiguousMatchException(string.Format(SR.Arg_AmbiguousMatchException_MemberInfo, declaringType, memberInfo));
        }

        internal static AmbiguousMatchException GetAmbiguousMatchException(Attribute attribute) {
            return new AmbiguousMatchException(string.Format(SR.Arg_AmbiguousMatchException_Attribute, attribute));
        }

        internal static AmbiguousMatchException GetAmbiguousMatchException(CustomAttributeData customAttributeData) {
            return new AmbiguousMatchException(string.Format(SR.Arg_AmbiguousMatchException_CustomAttributeData, customAttributeData));
        }
        */
        private static Exception GetArraySegmentCtorValidationFailedException(Array? array, int offset, int count) {
            if (array == null)
                return new ArgumentNullException(nameof(array));
            if (offset < 0)
                return new ArgumentOutOfRangeException(nameof(offset), /*SR.ArgumentOutOfRange_*/"NeedNonNegNum");
            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), /*SR.ArgumentOutOfRange_*/"NeedNonNegNum");

            Debug.Assert(array.Length - offset < count);
            return new ArgumentException(/*SR.Argument_*/"InvalidOffLen");
        }

        private static ArgumentException GetArgumentException(ExceptionResource resource) {
            return new ArgumentException(GetResourceString(resource));
        }

        private static InvalidOperationException GetInvalidOperationException(ExceptionResource resource) {
            return new InvalidOperationException(GetResourceString(resource));
        }

        private static ArgumentException GetWrongKeyTypeArgumentException(object? key, Type targetType) {
            return new ArgumentException(string.Format(/*SR.Arg_*/"WrongType", key, targetType), nameof(key));
        }

        private static ArgumentException GetWrongValueTypeArgumentException(object? value, Type targetType) {
            return new ArgumentException(string.Format(/*SR.Arg_*/"WrongType", value, targetType), nameof(value));
        }

        private static /*KeyNotFoundException*/SystemException GetKeyNotFoundException(object? key) {
            return new /*KeyNotFoundException*/SystemException(string.Format(/*SR.Arg_*/"KeyNotFoundWithKey {0}", key));
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        private static ArgumentException GetArgumentException(ExceptionResource resource, ExceptionArgument argument) {
            return new ArgumentException(GetResourceString(resource), GetArgumentName(argument));
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, int paramNumber, ExceptionResource resource) {
            return new ArgumentOutOfRangeException(GetArgumentName(argument) + "[" + paramNumber.ToString() + "]", GetResourceString(resource));
        }

        private static InvalidOperationException GetInvalidOperationException_EnumCurrent(int index) {
            return new InvalidOperationException(
                index < 0 ?
                /*SR.InvalidOperation_*/"EnumNotStarted" :
                /*SR.InvalidOperation_*/"EnumEnded");
        }

        /*
        // Allow nulls for reference types and Nullable<U>, but not for value types.
        // Aggressively inline so the jit evaluates the if in place and either drops the call altogether
        // Or just leaves null test and call to the Non-returning ThrowHelper.ThrowArgumentNullException
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IfNullAndNullsAreIllegalThenThrow<T>(object? value, ExceptionArgument argName) {
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>.
            if (!(default(T) == null) && value == null)
                ThrowArgumentNullException(argName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedSimdVectorBaseType<TVector, T>()
            where TVector : ISimdVector<TVector, T> {
            if (!TVector.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector<T> in the Numerics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedNumericsVectorBaseType<T>() {
            if (!Vector<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector64<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector64BaseType<T>() {
            if (!Vector64<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector128<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector128BaseType<T>() {
            if (!Vector128<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector256<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector256BaseType<T>() {
            if (!Vector256<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector512<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector512BaseType<T>() {
            if (!Vector512<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }
        */

#if false // Reflection-based implementation does not work for NativeAOT
        // This function will convert an ExceptionArgument enum value to the argument name string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetArgumentName(ExceptionArgument argument)
        {
            Debug.Assert(Enum.IsDefined(argument),
                "The enum value is not defined, please check the ExceptionArgument Enum.");

            return argument.ToString();
        }
#endif

        private static string GetArgumentName(ExceptionArgument argument) {
            switch (argument) {
            case ExceptionArgument.obj:
                return "obj";
            case ExceptionArgument.dictionary:
                return "dictionary";
            case ExceptionArgument.array:
                return "array";
            case ExceptionArgument.info:
                return "info";
            case ExceptionArgument.key:
                return "key";
            case ExceptionArgument.text:
                return "text";
            case ExceptionArgument.values:
                return "values";
            case ExceptionArgument.value:
                return "value";
            case ExceptionArgument.startIndex:
                return "startIndex";
            case ExceptionArgument.task:
                return "task";
            case ExceptionArgument.bytes:
                return "bytes";
            case ExceptionArgument.byteIndex:
                return "byteIndex";
            case ExceptionArgument.byteCount:
                return "byteCount";
            case ExceptionArgument.ch:
                return "ch";
            case ExceptionArgument.chars:
                return "chars";
            case ExceptionArgument.charIndex:
                return "charIndex";
            case ExceptionArgument.charCount:
                return "charCount";
            case ExceptionArgument.s:
                return "s";
            case ExceptionArgument.input:
                return "input";
            case ExceptionArgument.ownedMemory:
                return "ownedMemory";
            case ExceptionArgument.list:
                return "list";
            case ExceptionArgument.index:
                return "index";
            case ExceptionArgument.capacity:
                return "capacity";
            case ExceptionArgument.collection:
                return "collection";
            case ExceptionArgument.item:
                return "item";
            case ExceptionArgument.converter:
                return "converter";
            case ExceptionArgument.match:
                return "match";
            case ExceptionArgument.count:
                return "count";
            case ExceptionArgument.action:
                return "action";
            case ExceptionArgument.comparison:
                return "comparison";
            case ExceptionArgument.exceptions:
                return "exceptions";
            case ExceptionArgument.exception:
                return "exception";
            case ExceptionArgument.pointer:
                return "pointer";
            case ExceptionArgument.start:
                return "start";
            case ExceptionArgument.format:
                return "format";
            case ExceptionArgument.formats:
                return "formats";
            case ExceptionArgument.culture:
                return "culture";
            case ExceptionArgument.comparer:
                return "comparer";
            case ExceptionArgument.comparable:
                return "comparable";
            case ExceptionArgument.source:
                return "source";
            case ExceptionArgument.length:
                return "length";
            case ExceptionArgument.comparisonType:
                return "comparisonType";
            case ExceptionArgument.manager:
                return "manager";
            case ExceptionArgument.sourceBytesToCopy:
                return "sourceBytesToCopy";
            case ExceptionArgument.callBack:
                return "callBack";
            case ExceptionArgument.creationOptions:
                return "creationOptions";
            case ExceptionArgument.function:
                return "function";
            case ExceptionArgument.scheduler:
                return "scheduler";
            case ExceptionArgument.continuation:
                return "continuation";
            case ExceptionArgument.continuationAction:
                return "continuationAction";
            case ExceptionArgument.continuationFunction:
                return "continuationFunction";
            case ExceptionArgument.tasks:
                return "tasks";
            case ExceptionArgument.asyncResult:
                return "asyncResult";
            case ExceptionArgument.beginMethod:
                return "beginMethod";
            case ExceptionArgument.endMethod:
                return "endMethod";
            case ExceptionArgument.endFunction:
                return "endFunction";
            case ExceptionArgument.cancellationToken:
                return "cancellationToken";
            case ExceptionArgument.continuationOptions:
                return "continuationOptions";
            case ExceptionArgument.delay:
                return "delay";
            case ExceptionArgument.millisecondsDelay:
                return "millisecondsDelay";
            case ExceptionArgument.millisecondsTimeout:
                return "millisecondsTimeout";
            case ExceptionArgument.stateMachine:
                return "stateMachine";
            case ExceptionArgument.timeout:
                return "timeout";
            case ExceptionArgument.type:
                return "type";
            case ExceptionArgument.sourceIndex:
                return "sourceIndex";
            case ExceptionArgument.sourceArray:
                return "sourceArray";
            case ExceptionArgument.destinationIndex:
                return "destinationIndex";
            case ExceptionArgument.destinationArray:
                return "destinationArray";
            case ExceptionArgument.pHandle:
                return "pHandle";
            case ExceptionArgument.handle:
                return "handle";
            case ExceptionArgument.other:
                return "other";
            case ExceptionArgument.newSize:
                return "newSize";
            case ExceptionArgument.lengths:
                return "lengths";
            case ExceptionArgument.len:
                return "len";
            case ExceptionArgument.keys:
                return "keys";
            case ExceptionArgument.indices:
                return "indices";
            case ExceptionArgument.index1:
                return "index1";
            case ExceptionArgument.index2:
                return "index2";
            case ExceptionArgument.index3:
                return "index3";
            case ExceptionArgument.endIndex:
                return "endIndex";
            case ExceptionArgument.elementType:
                return "elementType";
            case ExceptionArgument.arrayIndex:
                return "arrayIndex";
            case ExceptionArgument.year:
                return "year";
            case ExceptionArgument.codePoint:
                return "codePoint";
            case ExceptionArgument.str:
                return "str";
            case ExceptionArgument.options:
                return "options";
            case ExceptionArgument.prefix:
                return "prefix";
            case ExceptionArgument.suffix:
                return "suffix";
            case ExceptionArgument.buffer:
                return "buffer";
            case ExceptionArgument.buffers:
                return "buffers";
            case ExceptionArgument.offset:
                return "offset";
            case ExceptionArgument.stream:
                return "stream";
            case ExceptionArgument.anyOf:
                return "anyOf";
            case ExceptionArgument.overlapped:
                return "overlapped";
            case ExceptionArgument.minimumBytes:
                return "minimumBytes";
            case ExceptionArgument.arrayType:
                return "arrayType";
            case ExceptionArgument.divisor:
                return "divisor";
            case ExceptionArgument.factor:
                return "factor";
            case ExceptionArgument.set:
                return "set";
            default:
                //Debug.Fail("The enum value is not defined, please check the ExceptionArgument Enum.");
                Debug.WriteLine("The enum value is not defined, please check the ExceptionArgument Enum.");
                Debug.Assert(false);
                return "";
            }
        }

#if false // Reflection-based implementation does not work for NativeAOT
        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ExceptionResource resource)
        {
            Debug.Assert(Enum.IsDefined(resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            return SR.GetResourceString(resource.ToString());
        }
#endif

        private static string GetResourceString(ExceptionResource resource) {
            switch (resource) {
            case ExceptionResource.ArgumentOutOfRange_IndexMustBeLessOrEqual:
                return /*SR.ArgumentOutOfRange_*/"IndexMustBeLessOrEqual";
            case ExceptionResource.ArgumentOutOfRange_IndexMustBeLess:
                return /*SR.ArgumentOutOfRange_*/"IndexMustBeLess";
            case ExceptionResource.ArgumentOutOfRange_IndexCount:
                return /*SR.ArgumentOutOfRange_*/"IndexCount";
            case ExceptionResource.ArgumentOutOfRange_IndexCountBuffer:
                return /*SR.ArgumentOutOfRange_*/"IndexCountBuffer";
            case ExceptionResource.ArgumentOutOfRange_Count:
                return /*SR.ArgumentOutOfRange_*/"Count";
            case ExceptionResource.ArgumentOutOfRange_Year:
                return /*SR.ArgumentOutOfRange_*/"Year";
            case ExceptionResource.Arg_ArrayPlusOffTooSmall:
                return /*SR.Arg_*/"ArrayPlusOffTooSmall";
            case ExceptionResource.Arg_ByteArrayTooSmallForValue:
                return /*SR.Arg_*/"ByteArrayTooSmallForValue";
            case ExceptionResource.NotSupported_ReadOnlyCollection:
                return /*SR.NotSupported_*/"ReadOnlyCollection";
            case ExceptionResource.Arg_RankMultiDimNotSupported:
                return /*SR.Arg_*/"RankMultiDimNotSupported";
            case ExceptionResource.Arg_NonZeroLowerBound:
                return /*SR.Arg_*/"NonZeroLowerBound";
            case ExceptionResource.ArgumentOutOfRange_GetCharCountOverflow:
                return /*SR.ArgumentOutOfRange_*/"GetCharCountOverflow";
            case ExceptionResource.ArgumentOutOfRange_ListInsert:
                return /*SR.ArgumentOutOfRange_*/"ListInsert";
            case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
                return /*SR.ArgumentOutOfRange_*/"NeedNonNegNum";
            case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
                return /*SR.ArgumentOutOfRange_*/"SmallCapacity";
            case ExceptionResource.Argument_InvalidOffLen:
                return /*SR.Argument_*/"InvalidOffLen";
            case ExceptionResource.Argument_CannotExtractScalar:
                return /*SR.Argument_*/"CannotExtractScalar";
            case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
                return /*SR.ArgumentOutOfRange_*/"BiggerThanCollection";
            case ExceptionResource.Serialization_MissingKeys:
                return /*SR.Serialization_*/"MissingKeys";
            case ExceptionResource.Serialization_NullKey:
                return /*SR.Serialization_*/"NullKey";
            case ExceptionResource.NotSupported_KeyCollectionSet:
                return /*SR.NotSupported_*/"KeyCollectionSet";
            case ExceptionResource.NotSupported_ValueCollectionSet:
                return /*SR.NotSupported_*/"ValueCollectionSet";
            case ExceptionResource.InvalidOperation_NullArray:
                return /*SR.InvalidOperation_*/"NullArray";
            case ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted:
                return /*SR.TaskT_*/"TransitionToFinal_AlreadyCompleted";
            case ExceptionResource.TaskCompletionSourceT_TrySetException_NullException:
                return /*SR.TaskCompletionSourceT_*/"TrySetException_NullException";
            case ExceptionResource.TaskCompletionSourceT_TrySetException_NoExceptions:
                return /*SR.TaskCompletionSourceT_*/"TrySetException_NoExceptions";
            case ExceptionResource.NotSupported_StringComparison:
                return /*SR.NotSupported_*/"StringComparison";
            case ExceptionResource.ConcurrentCollection_SyncRoot_NotSupported:
                return /*SR.ConcurrentCollection_*/"SyncRoot_NotSupported";
            case ExceptionResource.Task_MultiTaskContinuation_NullTask:
                return /*SR.Task_*/"MultiTaskContinuation_NullTask";
            case ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple:
                return /*SR.InvalidOperation_*/"WrongAsyncResultOrEndCalledMultiple";
            case ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList:
                return /*SR.Task_*/"MultiTaskContinuation_EmptyTaskList";
            case ExceptionResource.Task_Start_TaskCompleted:
                return /*SR.Task_*/"Start_TaskCompleted";
            case ExceptionResource.Task_Start_Promise:
                return /*SR.Task_*/"Start_Promise";
            case ExceptionResource.Task_Start_ContinuationTask:
                return /*SR.Task_*/"Start_ContinuationTask";
            case ExceptionResource.Task_Start_AlreadyStarted:
                return /*SR.Task_*/"Start_AlreadyStarted";
            case ExceptionResource.Task_RunSynchronously_Continuation:
                return /*SR.Task_*/"RunSynchronously_Continuation";
            case ExceptionResource.Task_RunSynchronously_Promise:
                return /*SR.Task_*/"RunSynchronously_Promise";
            case ExceptionResource.Task_RunSynchronously_TaskCompleted:
                return /*SR.Task_*/"RunSynchronously_TaskCompleted";
            case ExceptionResource.Task_RunSynchronously_AlreadyStarted:
                return /*SR.Task_*/"RunSynchronously_AlreadyStarted";
            case ExceptionResource.AsyncMethodBuilder_InstanceNotInitialized:
                return /*SR.AsyncMethodBuilder_*/"InstanceNotInitialized";
            case ExceptionResource.Task_ContinueWith_ESandLR:
                return /*SR.Task_*/"ContinueWith_ESandLR";
            case ExceptionResource.Task_ContinueWith_NotOnAnything:
                return /*SR.Task_*/"ContinueWith_NotOnAnything";
            case ExceptionResource.Task_InvalidTimerTimeSpan:
                return /*SR.Task_*/"InvalidTimerTimeSpan";
            case ExceptionResource.Task_Delay_InvalidMillisecondsDelay:
                return /*SR.Task_*/"Delay_InvalidMillisecondsDelay";
            case ExceptionResource.Task_Dispose_NotCompleted:
                return /*SR.Task_*/"Dispose_NotCompleted";
            case ExceptionResource.Task_ThrowIfDisposed:
                return /*SR.Task_*/"ThrowIfDisposed";
            case ExceptionResource.Task_WaitMulti_NullTask:
                return /*SR.Task_*/"WaitMulti_NullTask";
            case ExceptionResource.ArgumentException_OtherNotArrayOfCorrectLength:
                return /*SR.ArgumentException_*/"OtherNotArrayOfCorrectLength";
            case ExceptionResource.ArgumentNull_Array:
                return /*SR.ArgumentNull_*/"Array";
            case ExceptionResource.ArgumentNull_SafeHandle:
                return /*SR.ArgumentNull_*/"SafeHandle";
            case ExceptionResource.ArgumentOutOfRange_EndIndexStartIndex:
                return /*SR.ArgumentOutOfRange_*/"EndIndexStartIndex";
            case ExceptionResource.ArgumentOutOfRange_Enum:
                return /*SR.ArgumentOutOfRange_*/"Enum";
            case ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported:
                return /*SR.ArgumentOutOfRange_*/"HugeArrayNotSupported";
            case ExceptionResource.Argument_AddingDuplicate:
                return /*SR.Argument_*/"AddingDuplicate";
            case ExceptionResource.Argument_InvalidArgumentForComparison:
                return /*SR.Argument_*/"InvalidArgumentForComparison";
            case ExceptionResource.Arg_LowerBoundsMustMatch:
                return /*SR.Arg_*/"LowerBoundsMustMatch";
            case ExceptionResource.Arg_MustBeType:
                return /*SR.Arg_*/"MustBeType";
            case ExceptionResource.Arg_Need1DArray:
                return /*SR.Arg_*/"Need1DArray";
            case ExceptionResource.Arg_Need2DArray:
                return /*SR.Arg_*/"Need2DArray";
            case ExceptionResource.Arg_Need3DArray:
                return /*SR.Arg_*/"Need3DArray";
            case ExceptionResource.Arg_NeedAtLeast1Rank:
                return /*SR.Arg_*/"NeedAtLeast1Rank";
            case ExceptionResource.Arg_RankIndices:
                return /*SR.Arg_*/"RankIndices";
            case ExceptionResource.Arg_RanksAndBounds:
                return /*SR.Arg_*/"RanksAndBounds";
            case ExceptionResource.InvalidOperation_IComparerFailed:
                return /*SR.InvalidOperation_*/"IComparerFailed";
            case ExceptionResource.NotSupported_FixedSizeCollection:
                return /*SR.NotSupported_*/"FixedSizeCollection";
            case ExceptionResource.Rank_MultiDimNotSupported:
                return /*SR.Rank_*/"MultiDimNotSupported";
            case ExceptionResource.Arg_TypeNotSupported:
                return /*SR.Arg_*/"TypeNotSupported";
            case ExceptionResource.Argument_SpansMustHaveSameLength:
                return /*SR.Argument_*/"SpansMustHaveSameLength";
            case ExceptionResource.Argument_InvalidFlag:
                return /*SR.Argument_*/"InvalidFlag";
            case ExceptionResource.CancellationTokenSource_Disposed:
                return /*SR.CancellationTokenSource_*/"Disposed";
            case ExceptionResource.Argument_AlignmentMustBePow2:
                return /*SR.Argument_*/"AlignmentMustBePow2";
            case ExceptionResource.ArgumentOutOfRange_NotGreaterThanBufferLength:
                return /*SR.ArgumentOutOfRange_*/"NotGreaterThanBufferLength";
            case ExceptionResource.InvalidOperation_SpanOverlappedOperation:
                return /*SR.InvalidOperation_*/"SpanOverlappedOperation";
            case ExceptionResource.InvalidOperation_TimeProviderNullLocalTimeZone:
                return /*SR.InvalidOperation_*/"TimeProviderNullLocalTimeZone";
            case ExceptionResource.InvalidOperation_TimeProviderInvalidTimestampFrequency:
                return /*SR.InvalidOperation_*/"TimeProviderInvalidTimestampFrequency";
            case ExceptionResource.Format_UnexpectedClosingBrace:
                return /*SR.Format_*/"UnexpectedClosingBrace";
            case ExceptionResource.Format_UnclosedFormatItem:
                return /*SR.Format_*/"UnclosedFormatItem";
            case ExceptionResource.Format_ExpectedAsciiDigit:
                return /*SR.Format_*/"ExpectedAsciiDigit";
            case ExceptionResource.Argument_HasToBeArrayClass:
                return /*SR.Argument_*/"HasToBeArrayClass";
            case ExceptionResource.InvalidOperation_IncompatibleComparer:
                return /*SR.InvalidOperation_*/"IncompatibleComparer";
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
        obj,
        dictionary,
        array,
        info,
        key,
        text,
        values,
        value,
        startIndex,
        task,
        bytes,
        byteIndex,
        byteCount,
        ch,
        chars,
        charIndex,
        charCount,
        s,
        input,
        ownedMemory,
        list,
        index,
        capacity,
        collection,
        item,
        converter,
        match,
        count,
        action,
        comparison,
        exceptions,
        exception,
        pointer,
        start,
        format,
        formats,
        culture,
        comparer,
        comparable,
        source,
        length,
        comparisonType,
        manager,
        sourceBytesToCopy,
        callBack,
        creationOptions,
        function,
        scheduler,
        continuation,
        continuationAction,
        continuationFunction,
        tasks,
        asyncResult,
        beginMethod,
        endMethod,
        endFunction,
        cancellationToken,
        continuationOptions,
        delay,
        millisecondsDelay,
        millisecondsTimeout,
        stateMachine,
        timeout,
        type,
        sourceIndex,
        sourceArray,
        destinationIndex,
        destinationArray,
        pHandle,
        handle,
        other,
        newSize,
        lengths,
        len,
        keys,
        indices,
        index1,
        index2,
        index3,
        endIndex,
        elementType,
        arrayIndex,
        year,
        codePoint,
        str,
        options,
        prefix,
        suffix,
        buffer,
        buffers,
        offset,
        stream,
        anyOf,
        overlapped,
        minimumBytes,
        arrayType,
        divisor,
        factor,
        set,
    }

    //
    // The convention for this enum is using the resource name as the enum name
    //
    internal enum ExceptionResource {
        ArgumentOutOfRange_IndexMustBeLessOrEqual,
        ArgumentOutOfRange_IndexMustBeLess,
        ArgumentOutOfRange_IndexCount,
        ArgumentOutOfRange_IndexCountBuffer,
        ArgumentOutOfRange_Count,
        ArgumentOutOfRange_Year,
        Arg_ArrayPlusOffTooSmall,
        Arg_ByteArrayTooSmallForValue,
        NotSupported_ReadOnlyCollection,
        Arg_RankMultiDimNotSupported,
        Arg_NonZeroLowerBound,
        ArgumentOutOfRange_GetCharCountOverflow,
        ArgumentOutOfRange_ListInsert,
        ArgumentOutOfRange_NeedNonNegNum,
        ArgumentOutOfRange_NotGreaterThanBufferLength,
        ArgumentOutOfRange_SmallCapacity,
        Argument_InvalidOffLen,
        Argument_CannotExtractScalar,
        ArgumentOutOfRange_BiggerThanCollection,
        Serialization_MissingKeys,
        Serialization_NullKey,
        NotSupported_KeyCollectionSet,
        NotSupported_ValueCollectionSet,
        InvalidOperation_NullArray,
        TaskT_TransitionToFinal_AlreadyCompleted,
        TaskCompletionSourceT_TrySetException_NullException,
        TaskCompletionSourceT_TrySetException_NoExceptions,
        NotSupported_StringComparison,
        ConcurrentCollection_SyncRoot_NotSupported,
        Task_MultiTaskContinuation_NullTask,
        InvalidOperation_WrongAsyncResultOrEndCalledMultiple,
        Task_MultiTaskContinuation_EmptyTaskList,
        Task_Start_TaskCompleted,
        Task_Start_Promise,
        Task_Start_ContinuationTask,
        Task_Start_AlreadyStarted,
        Task_RunSynchronously_Continuation,
        Task_RunSynchronously_Promise,
        Task_RunSynchronously_TaskCompleted,
        Task_RunSynchronously_AlreadyStarted,
        AsyncMethodBuilder_InstanceNotInitialized,
        Task_ContinueWith_ESandLR,
        Task_ContinueWith_NotOnAnything,
        Task_InvalidTimerTimeSpan,
        Task_Delay_InvalidMillisecondsDelay,
        Task_Dispose_NotCompleted,
        Task_ThrowIfDisposed,
        Task_WaitMulti_NullTask,
        ArgumentException_OtherNotArrayOfCorrectLength,
        ArgumentNull_Array,
        ArgumentNull_SafeHandle,
        ArgumentOutOfRange_EndIndexStartIndex,
        ArgumentOutOfRange_Enum,
        ArgumentOutOfRange_HugeArrayNotSupported,
        Argument_AddingDuplicate,
        Argument_InvalidArgumentForComparison,
        Arg_LowerBoundsMustMatch,
        Arg_MustBeType,
        Arg_Need1DArray,
        Arg_Need2DArray,
        Arg_Need3DArray,
        Arg_NeedAtLeast1Rank,
        Arg_RankIndices,
        Arg_RanksAndBounds,
        InvalidOperation_IComparerFailed,
        NotSupported_FixedSizeCollection,
        Rank_MultiDimNotSupported,
        Arg_TypeNotSupported,
        Argument_SpansMustHaveSameLength,
        Argument_InvalidFlag,
        CancellationTokenSource_Disposed,
        Argument_AlignmentMustBePow2,
        InvalidOperation_SpanOverlappedOperation,
        InvalidOperation_TimeProviderNullLocalTimeZone,
        InvalidOperation_TimeProviderInvalidTimestampFrequency,
        Format_UnexpectedClosingBrace,
        Format_UnclosedFormatItem,
        Format_ExpectedAsciiDigit,
        Argument_HasToBeArrayClass,
        InvalidOperation_IncompatibleComparer,
    }
}