namespace System;

public static class EnumExts
{
    //static EnumExts()
    //{
    //}

    public static bool IsSet<TStruct, TCast>(this TStruct? bits, TStruct? flags)
        where TStruct : struct
        where TCast : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0 ||
            flags == null) return false;

        switch (typeof(TCast))
        {
            case Type _t when _t == ByteExts.Types.Byte:
            {
                var _bits = (byte)(object)bits;
                var result = (byte)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt16Exts.Types.UInt16:
            {
                var _bits = (ushort)(object)bits;
                var result = (ushort)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt32Exts.Types.UInt32:
            {
                var _bits = (uint)(object)bits;
                var result = (uint)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt64Exts.Types.UInt64:
            {
                var _bits = (ulong)(object)bits;
                var result = (ulong)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt128Exts.Types.UInt128:
            {
                var _bits = (UInt128)(object)bits;
                var result = (UInt128)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == SByteExts.Types.SByte:
            {
                var _bits = (sbyte)(object)bits;
                var result = (sbyte)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int16Exts.Types.Int16:
            {
                var _bits = (short)(object)bits;
                var result = (short)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int32Exts.Types.Int32:
            {
                var _bits = (int)(object)bits;
                var result = (int)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int64Exts.Types.Int64:
            {
                var _bits = (long)(object)bits;
                var result = (long)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int128Exts.Types.Int128:
            {
                var _bits = (Int128)(object)bits;
                var result = (Int128)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
        }

        return false;
    }

    public static bool IsSet<TEnum, TCast>(this TEnum? bits, TEnum? flags)
        where TEnum : Enum
        where TCast : struct
    {
        if (bits == null ||
            flags == null) return false;

        switch (typeof(TCast))
        {
            case Type _t when _t == ByteExts.Types.Byte:
            {
                var _bits = (byte)(object)bits;
                var result = (byte)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt16Exts.Types.UInt16:
            {
                var _bits = (ushort)(object)bits;
                var result = (ushort)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt32Exts.Types.UInt32:
            {
                var _bits = (uint)(object)bits;
                var result = (uint)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt64Exts.Types.UInt64:
            {
                var _bits = (ulong)(object)bits;
                var result = (ulong)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt128Exts.Types.UInt128:
            {
                var _bits = (UInt128)(object)bits;
                var result = (UInt128)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == SByteExts.Types.SByte:
            {
                var _bits = (sbyte)(object)bits;
                var result = (sbyte)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int16Exts.Types.Int16:
            {
                var _bits = (short)(object)bits;
                var result = (short)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int32Exts.Types.Int32:
            {
                var _bits = (int)(object)bits;
                var result = (int)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int64Exts.Types.Int64:
            {
                var _bits = (long)(object)bits;
                var result = (long)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int128Exts.Types.Int128:
            {
                var _bits = (Int128)(object)bits;
                var result = (Int128)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
        }

        return false;
    }

    public static bool IsSet<TEnum, TFlags, TCast>(this TEnum? bits, TFlags? flags)
        where TEnum : Enum
        where TFlags : struct
        where TCast : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0 ||
            flags == null) return false;

        switch (typeof(TCast))
        {
            case Type _t when _t == ByteExts.Types.Byte:
            {
                var _bits = (byte)(object)bits;
                var result = (byte)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt16Exts.Types.UInt16:
            {
                var _bits = (ushort)(object)bits;
                var result = (ushort)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt32Exts.Types.UInt32:
            {
                var _bits = (uint)(object)bits;
                var result = (uint)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt64Exts.Types.UInt64:
            {
                var _bits = (ulong)(object)bits;
                var result = (ulong)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt128Exts.Types.UInt128:
            {
                var _bits = (UInt128)(object)bits;
                var result = (UInt128)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == SByteExts.Types.SByte:
            {
                var _bits = (sbyte)(object)bits;
                var result = (sbyte)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int16Exts.Types.Int16:
            {
                var _bits = (short)(object)bits;
                var result = (short)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int32Exts.Types.Int32:
            {
                var _bits = (int)(object)bits;
                var result = (int)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int64Exts.Types.Int64:
            {
                var _bits = (long)(object)bits;
                var result = (long)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int128Exts.Types.Int128:
            {
                var _bits = (Int128)(object)bits;
                var result = (Int128)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
        }

        return false;
    }

    public static TStruct SetBit<TStruct>(this TStruct? bits, TStruct? flags, bool value)
        where TStruct : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0) return (TStruct)(object)0;

        var _b = (object)bits ?? default(TStruct);

        if (flags == null) return (TStruct)_b;

        var _f = (object)flags ?? default(TStruct);

        switch (typeof(TStruct))
        {
            case Type _t
                when _t == ByteExts.Types.Byte:
                return (TStruct)(object)(value ? (byte)_f & ~(byte)_b : (byte)_f | (byte)_b);
            case Type _t
                when _t == UInt16Exts.Types.UInt16:
                return (TStruct)(object)(value ? (ushort)_f & ~(ushort)_b : (ushort)_f | (ushort)_b);
            case Type _t
                when _t == UInt32Exts.Types.UInt32:
                return (TStruct)(object)(value ? (uint)_f & ~(uint)_b : (uint)_f | (uint)_b);
            case Type _t
                when _t == UInt64Exts.Types.UInt64:
                return (TStruct)(object)(value ? (ulong)_f & ~(ulong)_b : (ulong)_f | (ulong)_b);
            case Type _t when _t == UInt128Exts.Types.UInt128:
                return (TStruct)(object)(value ? (UInt128)_f & ~(UInt128)_b : (UInt128)_f | (UInt128)_b);
            case Type _t
                when _t == SByteExts.Types.SByte:
                return (TStruct)(object)(value ? (sbyte)_f & ~(sbyte)_b : (sbyte)_f | (sbyte)_b);
            case Type _t
                when _t == Int16Exts.Types.Int16:
                return (TStruct)(object)(value ? (short)_f & ~(short)_b : (short)_f | (short)_b);
            case Type _t
                when _t == Int32Exts.Types.Int32:
                return (TStruct)(object)(value ? (int)_f & ~(int)_b : (int)_f | (int)_b);
            case Type _t
                when _t == Int64Exts.Types.Int64:
                return (TStruct)(object)(value ? (long)_f & ~(long)_b : (long)_f | (long)_b);
            case Type _t
                when _t == Int128Exts.Types.Int128:
                return (TStruct)(object)(value ? (Int128)_f & ~(Int128)_b : (Int128)_f | (Int128)_b);
        }

        return (TStruct)(object)0;
    }

    public static TResult SetBit<TStruct, TResult>(this TStruct? bits, TStruct? flags, bool value)
        where TStruct : struct
        where TResult : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0) return (TResult)(object)0;

        var _b = (object)bits ?? default(TStruct);

        if (flags == null) return (TResult)_b;

        var _f = (object)flags ?? default(TStruct);

        switch (typeof(TStruct))
        {
            case Type _t
                when _t == ByteExts.Types.Byte:
                return (TResult)(object)(value ? (byte)_f & ~(byte)_b : (byte)_f | (byte)_b);
            case Type _t
                when _t == UInt16Exts.Types.UInt16:
                return (TResult)(object)(value ? (ushort)_f & ~(ushort)_b : (ushort)_f | (ushort)_b);
            case Type _t
                when _t == UInt32Exts.Types.UInt32:
                return (TResult)(object)(value ? (uint)_f & ~(uint)_b : (uint)_f | (uint)_b);
            case Type _t
                when _t == UInt64Exts.Types.UInt64:
                return (TResult)(object)(value ? (ulong)_f & ~(ulong)_b : (ulong)_f | (ulong)_b);
            case Type _t when _t == UInt128Exts.Types.UInt128:
                return (TResult)(object)(value ? (UInt128)_f & ~(UInt128)_b : (UInt128)_f | (UInt128)_b);
            case Type _t
                when _t == SByteExts.Types.SByte:
                return (TResult)(object)(value ? (sbyte)_f & ~(sbyte)_b : (sbyte)_f | (sbyte)_b);
            case Type _t
                when _t == Int16Exts.Types.Int16:
                return (TResult)(object)(value ? (short)_f & ~(short)_b : (short)_f | (short)_b);
            case Type _t
                when _t == Int32Exts.Types.Int32:
                return (TResult)(object)(value ? (int)_f & ~(int)_b : (int)_f | (int)_b);
            case Type _t
                when _t == Int64Exts.Types.Int64:
                return (TResult)(object)(value ? (long)_f & ~(long)_b : (long)_f | (long)_b);
            case Type _t
                when _t == Int128Exts.Types.Int128:
                return (TResult)(object)(value ? (Int128)_f & ~(Int128)_b : (Int128)_f | (Int128)_b);
        }

        return (TResult)(object)0;
    }

    public static TEnum SetBit<TEnum, TCast>(this TEnum? bits, TEnum? flags, bool value)
        where TEnum : Enum
        where TCast : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0) return (TEnum)(object)0;

        var _b = (object)bits ?? default(TEnum);

        if (flags == null) return (TEnum)_b;

        var _f = (object)flags ?? default(TEnum);

        switch (typeof(TCast))
        {
            case Type _t
                when _t == ByteExts.Types.Byte:
                return (TEnum)(object)(value ? (byte)_f & ~(byte)_b : (byte)_f | (byte)_b);
            case Type _t
                when _t == UInt16Exts.Types.UInt16:
                return (TEnum)(object)(value ? (ushort)_f & ~(ushort)_b : (ushort)_f | (ushort)_b);
            case Type _t
                when _t == UInt32Exts.Types.UInt32:
                return (TEnum)(object)(value ? (uint)_f & ~(uint)_b : (uint)_f | (uint)_b);
            case Type _t
                when _t == UInt64Exts.Types.UInt64:
                return (TEnum)(object)(value ? (ulong)_f & ~(ulong)_b : (ulong)_f | (ulong)_b);
            case Type _t
                when _t == UInt128Exts.Types.UInt128:
                return (TEnum)(object)(value ? (UInt128)_f & ~(UInt128)_b : (UInt128)_f | (UInt128)_b);
            case Type _t
                when _t == SByteExts.Types.SByte:
                return (TEnum)(object)(value ? (sbyte)_f & ~(sbyte)_b : (sbyte)_f | (sbyte)_b);
            case Type _t
                when _t == Int16Exts.Types.Int16:
                return (TEnum)(object)(value ? (short)_f & ~(short)_b : (short)_f | (short)_b);
            case Type _t
                when _t == Int32Exts.Types.Int32:
                return (TEnum)(object)(value ? (int)_f & ~(int)_b : (int)_f | (int)_b);
            case Type _t
                when _t == Int64Exts.Types.Int64:
                return (TEnum)(object)(value ? (long)_f & ~(long)_b : (long)_f | (long)_b);
            case Type _t
                when _t == Int128Exts.Types.Int128:
                return (TEnum)(object)(value ? (Int128)_f & ~(Int128)_b : (Int128)_f | (Int128)_b);
        }

        return (TEnum)(object)0;
    }

    public static TResult SetBit<TEnum, TCast, TResult>(this TEnum? bits, TEnum? flags, bool value)
        where TEnum : Enum
        where TCast : struct
        where TResult : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0) return (TResult)(object)0;

        var _b = (object)bits ?? default(TEnum);

        if (flags == null) return (TResult)_b;

        var _f = (object)flags ?? default(TEnum);

        switch (typeof(TCast))
        {
            case Type _t
                when _t == ByteExts.Types.Byte:
                return (TResult)(object)(value ? (byte)_f & ~(byte)_b : (byte)_f | (byte)_b);
            case Type _t
                when _t == UInt16Exts.Types.UInt16:
                return (TResult)(object)(value ? (ushort)_f & ~(ushort)_b : (ushort)_f | (ushort)_b);
            case Type _t
                when _t == UInt32Exts.Types.UInt32:
                return (TResult)(object)(value ? (uint)_f & ~(uint)_b : (uint)_f | (uint)_b);
            case Type _t
                when _t == UInt64Exts.Types.UInt64:
                return (TResult)(object)(value ? (ulong)_f & ~(ulong)_b : (ulong)_f | (ulong)_b);
            case Type _t when _t == UInt128Exts.Types.UInt128:
                return (TResult)(object)(value ? (UInt128)_f & ~(UInt128)_b : (UInt128)_f | (UInt128)_b);
            case Type _t
                when _t == SByteExts.Types.SByte:
                return (TResult)(object)(value ? (sbyte)_f & ~(sbyte)_b : (sbyte)_f | (sbyte)_b);
            case Type _t
                when _t == Int16Exts.Types.Int16:
                return (TResult)(object)(value ? (short)_f & ~(short)_b : (short)_f | (short)_b);
            case Type _t
                when _t == Int32Exts.Types.Int32:
                return (TResult)(object)(value ? (int)_f & ~(int)_b : (int)_f | (int)_b);
            case Type _t
                when _t == Int64Exts.Types.Int64:
                return (TResult)(object)(value ? (long)_f & ~(long)_b : (long)_f | (long)_b);
            case Type _t
                when _t == Int128Exts.Types.Int128:
                return (TResult)(object)(value ? (Int128)_f & ~(Int128)_b : (Int128)_f | (Int128)_b);
        }

        return (TResult)(object)0;
    }

    public static TResult SetBit<TEnum, TFlags, TCast, TResult>(this TEnum? bits, TFlags? flags, bool value)
        where TEnum : Enum
        where TFlags : struct
        where TCast : struct
        where TResult : struct
    {
        if (bits == null ||
            (ulong)(object)bits == 0) return (TResult)(object)0;

        var _b = (object)bits ?? default(TEnum);

        if (flags == null) return (TResult)_b;

        var _f = (object)flags ?? default(TFlags);

        switch (typeof(TCast))
        {
            case Type _t
                when _t == ByteExts.Types.Byte:
                return (TResult)(object)(value ? (byte)_f & ~(byte)_b : (byte)_f | (byte)_b);
            case Type _t
                when _t == UInt16Exts.Types.UInt16:
                return (TResult)(object)(value ? (ushort)_f & ~(ushort)_b : (ushort)_f | (ushort)_b);
            case Type _t
                when _t == UInt32Exts.Types.UInt32:
                return (TResult)(object)(value ? (uint)_f & ~(uint)_b : (uint)_f | (uint)_b);
            case Type _t
                when _t == UInt64Exts.Types.UInt64:
                return (TResult)(object)(value ? (ulong)_f & ~(ulong)_b : (ulong)_f | (ulong)_b);
            case Type _t when _t == UInt128Exts.Types.UInt128:
                return (TResult)(object)(value ? (UInt128)_f & ~(UInt128)_b : (UInt128)_f | (UInt128)_b);
            case Type _t
                when _t == SByteExts.Types.SByte:
                return (TResult)(object)(value ? (sbyte)_f & ~(sbyte)_b : (sbyte)_f | (sbyte)_b);
            case Type _t
                when _t == Int16Exts.Types.Int16:
                return (TResult)(object)(value ? (short)_f & ~(short)_b : (short)_f | (short)_b);
            case Type _t
                when _t == Int32Exts.Types.Int32:
                return (TResult)(object)(value ? (int)_f & ~(int)_b : (int)_f | (int)_b);
            case Type _t
                when _t == Int64Exts.Types.Int64:
                return (TResult)(object)(value ? (long)_f & ~(long)_b : (long)_f | (long)_b);
            case Type _t
                when _t == Int128Exts.Types.Int128:
                return (TResult)(object)(value ? (Int128)_f & ~(Int128)_b : (Int128)_f | (Int128)_b);
        }

        return (TResult)(object)0;
    }

    public static class Types
    {
        public static readonly Type Enum = typeof(Enum);
        public static readonly Type EnumArray = typeof(Enum[]);
        public static readonly Type EnumList = typeof(List<Enum>);
    }
}