// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

// In AOT compilers, see Internal.IL.Stubs.UnsafeIntrinsics for details.

namespace UltimateOrb.nanoFramework.Runtime.CompilerServices {

    /// <summary>
    /// Contains generic, low-level functionality for manipulating pointers.
    /// </summary>
    public static partial class Unsafe {

        /// <summary>
        /// Copies a value of type T to the given location.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__PTR_BYREF_COPY
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void CopyBlock(IntPtr destination, ref /*readonly */byte source, uint byteCount);

        /// <summary>
        /// Copies a value of type T to the given location.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_PTR_COPY
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void CopyBlock(ref byte destination, IntPtr source, uint byteCount);

        /// <summary>
        /// Copies bytes from the source address to the destination address.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__PTR_COPY_BLOCK
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void CopyBlock(IntPtr destination, IntPtr source, uint byteCount);

        /// <summary>
        /// Copies bytes from the source address to the destination address.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_COPY_BLOCK
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void CopyBlock(ref byte destination, ref /*readonly */byte source, uint byteCount);

        /*
        /// <summary>
        /// Determines whether the memory address referenced by <paramref name="left"/> is greater than
        /// the memory address referenced by <paramref name="right"/>.
        /// </summary>
        /// <remarks>
        /// This check is conceptually similar to "(void*)(&amp;left) &gt; (void*)(&amp;right)".
        /// </remarks>
        [Intrinsic]
        // CoreCLR:CoreCLR:METHOD__UNSAFE__BYREF_IS_ADDRESS_GREATER_THAN
        // AOT:IsAddressGreaterThan
        // Mono:IsAddressGreaterThan
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGreaterThan<T>([AllowNull] ref readonly T left, [AllowNull] ref readonly T right)
            where T : allows ref struct {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ldarg.1
            // cgt.un
            // ret
        }

        /// <summary>
        /// Determines whether the memory address referenced by <paramref name="left"/> is less than
        /// the memory address referenced by <paramref name="right"/>.
        /// </summary>
        /// <remarks>
        /// This check is conceptually similar to "(void*)(&amp;left) &lt; (void*)(&amp;right)".
        /// </remarks>
        [Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_IS_ADDRESS_LESS_THAN
        // AOT:IsAddressLessThan
        // Mono:IsAddressLessThan
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLessThan<T>([AllowNull] ref readonly T left, [AllowNull] ref readonly T right)
            where T : allows ref struct {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ldarg.1
            // clt.un
            // ret
        }
        
        */
        /// <summary>
        /// Initializes a block of memory at the given location with a given initial value.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__PTR_INIT_BLOCK
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void InitBlock(IntPtr startAddress, byte value, uint byteCount);

        /// <summary>
        /// Initializes a block of memory at the given location with a given initial value.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_INIT_BLOCK
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void InitBlock(ref byte startAddress, byte value, uint byteCount);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool ReadBoolean(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static char ReadChar(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Single ReadSingle(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double ReadDouble(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Int16 ReadInt16(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Int32 ReadInt32(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Int64 ReadInt64(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static UInt16 ReadUInt16(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static UInt32 ReadUInt32(IntPtr source);

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        //[CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static UInt64 ReadUInt64(IntPtr source);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteBoolean(IntPtr destination, bool value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteChar(IntPtr destination, char value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteSingle(IntPtr destination, Single value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteDouble(IntPtr destination, double value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteInt16(IntPtr destination, Int16 value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteInt32(IntPtr destination, Int32 value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteInt64(IntPtr destination, Int64 value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteUInt16(IntPtr destination, UInt16 value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteUInt32(IntPtr destination, UInt32 value);

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        //[Intrinsic]
        //[NonVersionable]
        [CLSCompliant(false)]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void WriteUInt64(IntPtr destination, UInt64 value);


        /// <summary>
        /// Determines the byte offset from origin to target from the given references.
        /// </summary>
        //[Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_BYTE_OFFSET
        // AOT:ByteOffset
        // Mono:ByteOffset
        //[NonVersionable]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[MethodImpl(MethodImplOptions.InternalCall)]
        //public extern static nint ByteOffset(/*[AllowNull] */ref /*readonly */byte origin, /*[AllowNull] */ref /*readonly */byte target);

        /*
        /// <summary>
        /// Returns a by-ref to type <typeparamref name="T"/> that is a null reference.
        /// </summary>
        [Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_NULLREF
        // AOT:NullRef
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NullRef<T>()
            where T : allows ref struct {
            return ref AsRef<T>(null);

            // ldc.i4.0
            // conv.u
            // ret
        }
        */

        /* Not very useful
        /// <summary>
        /// Returns if a given by-ref to type <typeparamref name="T"/> is a null reference.
        /// </summary>
        /// <remarks>
        /// This check is conceptually similar to "(void*)(&amp;source) == nullptr".
        /// </remarks>
        [Intrinsic]
        // CoreCLR:METHOD__UNSAFE__BYREF_IS_NULL
        // AOT: IsNullRef
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullRef<T>(ref readonly T source)
            where T : allows ref struct {
            return AsPointer(ref Unsafe.AsRef(in source)) == null;

            // ldarg.0
            // ldc.i4.0
            // conv.u
            // ceq
            // ret
        }
        */

        /*
        /// <summary>
        /// Bypasses definite assignment rules by taking advantage of <c>out</c> semantics.
        /// </summary>
        [Intrinsic]
        // CoreCLR:METHOD__UNSAFE__SKIPINIT
        // AOT:SkipInit
        // Mono:SkipInit
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SkipInit<T>(out T value)
            where T : allows ref struct {
            throw new PlatformNotSupportedException();

            // ret
        }
        */

        /*
        /// <summary>
        /// Returns a mutable ref to a boxed value
        /// </summary>
        [Intrinsic]
        // CoreCLR:METHOD__UNSAFE__UNBOX
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Unbox<T>(object box)
            where T : struct {
            throw new PlatformNotSupportedException();

            // ldarg .0
            // unbox !!T
            // ret
        }
        */

        /*
        // Internal helper methods:

        // Determines if the address is aligned at least to `alignment` bytes.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsOpportunisticallyAligned<T>(ref readonly T address, nuint alignment) {
            // `alignment` is expected to be a power of 2 in bytes.
            // We use Unsafe.AsPointer to convert to a pointer,
            // GC will keep alignment when moving objects (up to sizeof(void*)),
            // otherwise alignment should be considered a hint if not pinned.
            Debug.Assert(nuint.IsPow2(alignment));
            return ((nuint)AsPointer(ref AsRef(in address)) & (alignment - 1)) == 0;
        }

        // Determines the misalignment of the address with respect to the specified `alignment`.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static nuint OpportunisticMisalignment<T>(ref readonly T address, nuint alignment) {
            // `alignment` is expected to be a power of 2 in bytes.
            // We use Unsafe.AsPointer to convert to a pointer,
            // GC will keep alignment when moving objects (up to sizeof(void*)),
            // otherwise alignment should be considered a hint if not pinned.
            Debug.Assert(nuint.IsPow2(alignment));
            return (nuint)AsPointer(ref AsRef(in address)) & (alignment - 1);
        }
        */
    }
}