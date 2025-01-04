// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.ThrowHelper;

[assembly: CLSCompliant(true)]

namespace System.Runtime.InteropServices {

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
    using IntPtr = UltimateOrb.nanoFramework.IntPtr;
    using nuint = UltimateOrb.nanoFramework.nuint;
#endif

    public static partial class NativeMemory {

        /// <summary>Allocates a block of memory of the specified size, in elements.</summary>
        /// <param name="elementCount">The count, in elements, of the block to allocate.</param>
        /// <param name="elementSize">The size, in bytes, of each element in the allocation.</param>
        /// <returns>A pointer to the allocated block of memory.</returns>
        /// <exception cref="OutOfMemoryException">Allocating <paramref name="elementCount" /> * <paramref name="elementSize" /> bytes of memory failed.</exception>
        /// <remarks>
        ///     <para>This method allows <paramref name="elementCount" /> and/or <paramref name="elementSize" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a thin wrapper over the C <c>malloc</c> API.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr Alloc(nuint elementCount, nuint elementSize) {
            nuint byteCount = GetByteCount(elementCount, elementSize);
            return Alloc(byteCount);
        }

        /// <summary>Allocates and zeroes a block of memory of the specified size, in bytes.</summary>
        /// <param name="byteCount">The size, in bytes, of the block to allocate.</param>
        /// <returns>A pointer to the allocated and zeroed block of memory.</returns>
        /// <exception cref="OutOfMemoryException">Allocating <paramref name="byteCount" /> of memory failed.</exception>
        /// <remarks>
        ///     <para>This method allows <paramref name="byteCount" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a thin wrapper over the C <c>calloc</c> API.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr AllocZeroed(nuint byteCount) {
            return AllocZeroed(byteCount, elementSize: 1);
        }

        /// <summary>Clears a block of memory.</summary>
        /// <param name="ptr">A pointer to the block of memory that should be cleared.</param>
        /// <param name="byteCount">The size, in bytes, of the block to clear.</param>
        /// <remarks>
        ///     <para>If this method is called with <paramref name="ptr" /> being <see langword="null"/> and <paramref name="byteCount" /> being <c>0</c>, it will be equivalent to a no-op.</para>
        ///     <para>The behavior when <paramref name="ptr" /> is <see langword="null"/> and <paramref name="byteCount" /> is greater than <c>0</c> is undefined.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public extern static void Clear(IntPtr ptr, nuint byteCount);

        /// <summary>
        /// Copies a block of memory from memory location <paramref name="source"/>
        /// to memory location <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">A pointer to the source of data to be copied.</param>
        /// <param name="destination">A pointer to the destination memory block where the data is to be copied.</param>
        /// <param name="byteCount">The size, in bytes, to be copied from the source location to the destination.</param>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Copy(IntPtr source, IntPtr destination, nuint byteCount);

        /// <summary>
        /// Copies the byte <paramref name="value"/> to the first <paramref name="byteCount"/> bytes
        /// of the memory located at <paramref name="ptr"/>.
        /// </summary>
        /// <param name="ptr">A pointer to the block of memory to fill.</param>
        /// <param name="byteCount">The number of bytes to be set to <paramref name="value"/>.</param>
        /// <param name="value">The value to be set.</param>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Fill(IntPtr ptr, nuint byteCount, byte value);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint GetByteCount(nuint elementCount, nuint elementSize) {
            // This is based on the `mi_count_size_overflow` and `mi_mul_overflow` methods from microsoft/mimalloc.
            // Original source is Copyright (c) 2019 Microsoft Corporation, Daan Leijen. Licensed under the MIT license

            // sqrt(nuint.MaxValue)
#if USE_WRAPPED_NATIVE_INTEGER_TYPES
            nuint multiplyNoOverflow = (nuint)1 << (4 * nuint.Size);
#else
            nuint multiplyNoOverflow = (nuint)1 << (4 * nuint_Size);
#endif

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
            return ((elementSize >= multiplyNoOverflow) || (elementCount >= multiplyNoOverflow)) && (elementSize > 0) && (nuint.MaxValue / elementSize < elementCount) ? nuint.MaxValue : (elementCount * elementSize);
#else
            return ((elementSize >= multiplyNoOverflow) || (elementCount >= multiplyNoOverflow)) && (elementSize > 0) && (nuint_MaxValue / elementSize < elementCount) ? nuint_MaxValue : (elementCount * elementSize);
#endif
        }

#if !USE_WRAPPED_NATIVE_INTEGER_TYPES
#if TARGET_64BIT
        public static nuint_MaxValue {

            get => unchecked((nuint)UInt64.MaxValue);
        }
#else
        const nuint nuint_MaxValue = (nuint)uint.MaxValue;
#endif
        const int nuint_Size = 4;
#endif
    }
}

namespace System.Runtime.InteropServices {

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
    using IntPtr = UltimateOrb.nanoFramework.IntPtr;
    using nuint = UltimateOrb.nanoFramework.nuint;
#endif

    /// <summary>This class contains methods that are mainly used to manage native memory.</summary>
    public static partial class NativeMemory {
        /// <summary>Allocates an aligned block of memory of the specified size and alignment, in bytes.</summary>
        /// <param name="byteCount">The size, in bytes, of the block to allocate.</param>
        /// <param name="alignment">The alignment, in bytes, of the block to allocate. This must be a power of <c>2</c>.</param>
        /// <returns>A pointer to the allocated aligned block of memory.</returns>
        /// <exception cref="ArgumentException"><paramref name="alignment" /> is not a power of two.</exception>
        /// <exception cref="OutOfMemoryException">Allocating <paramref name="byteCount" /> of memory with <paramref name="alignment" /> failed.</exception>
        /// <remarks>
        ///     <para>This method allows <paramref name="byteCount" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a thin wrapper over the C <c>aligned_alloc</c> API or a platform dependent aligned allocation API such as <c>_aligned_malloc</c> on Win32.</para>
        ///     <para>This method is not compatible with <see cref="Free" /> or <see cref="Realloc" />, instead <see cref="AlignedFree" /> or <see cref="AlignedRealloc" /> should be called.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr AlignedAlloc(nuint byteCount, nuint alignment) {
            if (!BitOperations.IsPow2(alignment)) {
                // The C standard doesn't define what a valid alignment is, however Windows and POSIX requires a power of 2
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlignmentMustBePow2);
            }

            // The C standard and POSIX requires size to be a multiple of alignment and we want an "empty" allocation for zero
            // POSIX additionally requires alignment to be at least sizeof(IntPtr)

            // The adjustment for byteCount can overflow here, and such overflow is generally "harmless". This is because of the
            // requirement that alignment be a power of two and that byteCount be a multiple of alignment. Given both of these
            // constraints we should only overflow for byteCount > (nuint.MaxValue & ~(alignment - 1)). When such an overflow
            // occurs we will get a result that is less than alignment which will cause the allocation to fail.
            //
            // However, posix_memalign differs from aligned_alloc in that it may return a valid pointer for zero and we need to
            // ensure we OOM for this scenario (which can occur for `nuint.MaxValue`) and so we have to check the adjusted size.

#if USE_WRAPPED_NATIVE_INTEGER_TYPES
            nuint adjustedAlignment = Math.Max(alignment, (uint)IntPtr.Size);
#else
            nuint adjustedAlignment = Math.Max(alignment, (uint)IntPtr_Size);
#endif
            nuint adjustedByteCount = (byteCount != 0) ? (byteCount + (adjustedAlignment - 1)) & ~(adjustedAlignment - 1) : adjustedAlignment;

            IntPtr result = (adjustedByteCount < byteCount) ? default : Interop.Sys.AlignedAlloc(adjustedAlignment, adjustedByteCount);

            if ((nint)result == default) {
                ThrowHelper.ThrowOutOfMemoryException();
            }

            return result;
        }

#if !USE_WRAPPED_NATIVE_INTEGER_TYPES
        const int IntPtr_Size = 4;
#endif

        /// <summary>Frees an aligned block of memory.</summary>
        /// <param name="ptr">A pointer to the aligned block of memory that should be freed.</param>
        /// <remarks>
        ///    <para>This method does nothing if <paramref name="ptr" /> is <c>null</c>.</para>
        ///    <para>This method is a thin wrapper over the C <c>free</c> API or a platform dependent aligned free API such as <c>_aligned_free</c> on Win32.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static void AlignedFree(IntPtr ptr) {
            if ((nint)ptr != default) {
                Interop.Sys.AlignedFree(ptr);
            }
        }

        /// <summary>Reallocates an aligned block of memory of the specified size and alignment, in bytes.</summary>
        /// <param name="ptr">The previously allocated block of memory.</param>
        /// <param name="byteCount">The size, in bytes, of the block to allocate.</param>
        /// <param name="alignment">The alignment, in bytes, of the block to allocate. This must be a power of <c>2</c>.</param>
        /// <returns>A pointer to the reallocated aligned block of memory.</returns>
        /// <exception cref="ArgumentException"><paramref name="alignment" /> is not a power of two.</exception>
        /// <exception cref="OutOfMemoryException">Reallocating <paramref name="byteCount" /> of memory with <paramref name="alignment" /> failed.</exception>
        /// <remarks>
        ///     <para>This method acts as <see cref="AlignedAlloc" /> if <paramref name="ptr" /> is <c>null</c>.</para>
        ///     <para>This method allows <paramref name="byteCount" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a platform dependent aligned reallocation API such as <c>_aligned_realloc</c> on Win32.</para>
        ///     <para>This method is not compatible with <see cref="Free" /> or <see cref="Realloc" />, instead <see cref="AlignedFree" /> or <see cref="AlignedRealloc" /> should be called.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr AlignedRealloc(IntPtr ptr, nuint byteCount, nuint alignment) {
            if (!BitOperations.IsPow2(alignment)) {
                // The C standard doesn't define what a valid alignment is, however Windows and POSIX requires a power of 2
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AlignmentMustBePow2);
            }

            // The C standard and POSIX requires size to be a multiple of alignment and we want an "empty" allocation for zero
            // POSIX additionally requires alignment to be at least sizeof(IntPtr)

            // The adjustment for byteCount can overflow here, and such overflow is generally "harmless". This is because of the
            // requirement that alignment be a power of two and that byteCount be a multiple of alignment. Given both of these
            // constraints we should only overflow for byteCount > (nuint.MaxValue & ~(alignment - 1)). When such an overflow
            // occurs we will get a result that is less than alignment which will cause the allocation to fail.
            //
            // However, posix_memalign differs from aligned_alloc in that it may return a valid pointer for zero and we need to
            // ensure we OOM for this scenario (which can occur for `nuint.MaxValue`) and so we have to check the adjusted size.


#if USE_WRAPPED_NATIVE_INTEGER_TYPES
            nuint adjustedAlignment = Math.Max(alignment, (uint)IntPtr.Size);
#else
            nuint adjustedAlignment = Math.Max(alignment, (uint)IntPtr_Size);
#endif
            nuint adjustedByteCount = (byteCount != 0) ? (byteCount + (adjustedAlignment - 1)) & ~(adjustedAlignment - 1) : adjustedAlignment;

            IntPtr result = (adjustedByteCount < byteCount) ? default : Interop.Sys.AlignedRealloc(ptr, adjustedAlignment, adjustedByteCount);

            if ((nint)result == default) {
                ThrowHelper.ThrowOutOfMemoryException();
            }

            return result;
        }

        /// <summary>Allocates a block of memory of the specified size, in bytes.</summary>
        /// <param name="byteCount">The size, in bytes, of the block to allocate.</param>
        /// <returns>A pointer to the allocated block of memory.</returns>
        /// <exception cref="OutOfMemoryException">Allocating <paramref name="byteCount" /> of memory failed.</exception>
        /// <remarks>
        ///     <para>This method allows <paramref name="byteCount" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a thin wrapper over the C <c>malloc</c> API.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr Alloc(nuint byteCount) {
            // The C standard does not define what happens when size == 0, we want an "empty" allocation
            IntPtr result = Interop.Sys.Malloc((byteCount != 0) ? byteCount : 1);

            if ((nint)result == default) {
                ThrowHelper.ThrowOutOfMemoryException();
            }

            return result;
        }

        /// <summary>Allocates and zeroes a block of memory of the specified size, in elements.</summary>
        /// <param name="elementCount">The count, in elements, of the block to allocate.</param>
        /// <param name="elementSize">The size, in bytes, of each element in the allocation.</param>
        /// <returns>A pointer to the allocated and zeroed block of memory.</returns>
        /// <exception cref="OutOfMemoryException">Allocating <paramref name="elementCount" /> * <paramref name="elementSize" /> bytes of memory failed.</exception>
        /// <remarks>
        ///     <para>This method allows <paramref name="elementCount" /> and/or <paramref name="elementSize" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a thin wrapper over the C <c>calloc</c> API.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr AllocZeroed(nuint elementCount, nuint elementSize) {
            IntPtr result;

            if ((elementCount != 0) && (elementSize != 0)) {
                result = Interop.Sys.Calloc(elementCount, elementSize);
            } else {
                // The C standard does not define what happens when num == 0 or size == 0, we want an "empty" allocation
                result = Interop.Sys.Malloc(1);
            }

            if ((nint)result == default) {
                ThrowHelper.ThrowOutOfMemoryException();
            }

            return result;
        }

        /// <summary>Frees a block of memory.</summary>
        /// <param name="ptr">A pointer to the block of memory that should be freed.</param>
        /// <remarks>
        ///    <para>This method does nothing if <paramref name="ptr" /> is <c>null</c>.</para>
        ///    <para>This method is a thin wrapper over the C <c>free</c> API.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static void Free(IntPtr ptr) {
            if ((nint)ptr != default) {
                Interop.Sys.Free(ptr);
            }
        }

        /// <summary>Reallocates a block of memory to be the specified size, in bytes.</summary>
        /// <param name="ptr">The previously allocated block of memory.</param>
        /// <param name="byteCount">The size, in bytes, of the reallocated block.</param>
        /// <returns>A pointer to the reallocated block of memory.</returns>
        /// <exception cref="OutOfMemoryException">Reallocating <paramref name="byteCount" /> of memory failed.</exception>
        /// <remarks>
        ///     <para>This method acts as <see cref="Alloc" /> if <paramref name="ptr" /> is <c>null</c>.</para>
        ///     <para>This method allows <paramref name="byteCount" /> to be <c>0</c> and will return a valid pointer that should not be dereferenced and that should be passed to free to avoid memory leaks.</para>
        ///     <para>This method is a thin wrapper over the C <c>realloc</c> API.</para>
        /// </remarks>
        [CLSCompliant(false)]
        public static IntPtr Realloc(IntPtr ptr, nuint byteCount) {
            // The C standard does not define what happens when size == 0, we want an "empty" allocation
            IntPtr result = Interop.Sys.Realloc(ptr, (byteCount != 0) ? byteCount : 1);

            if ((nint)result == default) {
                ThrowHelper.ThrowOutOfMemoryException();
            }

            return result;
        }
    }
}
