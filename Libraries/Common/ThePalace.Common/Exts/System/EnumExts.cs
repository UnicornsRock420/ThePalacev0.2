namespace System;

public static class EnumExts
{
    public static class Types
    {
        public static readonly Type Enum = typeof(Enum);
        public static readonly Type EnumArray = typeof(Enum[]);
        public static readonly Type EnumList = typeof(List<Enum>);
    }

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
                var _bits = (Byte)(object)bits;
                var result = (Byte)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt16Exts.Types.UInt16:
            {
                var _bits = (UInt16)(object)bits;
                var result = (UInt16)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt32Exts.Types.UInt32:
            {
                var _bits = (UInt32)(object)bits;
                var result = (UInt32)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt64Exts.Types.UInt64:
            {
                var _bits = (UInt64)(object)bits;
                var result = (UInt64)(object)(flags ?? default(TStruct)) & _bits;
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
                var _bits = (SByte)(object)bits;
                var result = (SByte)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int16Exts.Types.Int16:
            {
                var _bits = (Int16)(object)bits;
                var result = (Int16)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int32Exts.Types.Int32:
            {
                var _bits = (Int32)(object)bits;
                var result = (Int32)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int64Exts.Types.Int64:
            {
                var _bits = (Int64)(object)bits;
                var result = (Int64)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int128Exts.Types.Int128:
            {
                var _bits = (Int128)(object)bits;
                var result = (Int128)(object)(flags ?? default(TStruct)) & _bits;
                return result == _bits && result != 0;
            }
            default: break;
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
                var _bits = (Byte)(object)bits;
                var result = (Byte)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt16Exts.Types.UInt16:
            {
                var _bits = (UInt16)(object)bits;
                var result = (UInt16)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt32Exts.Types.UInt32:
            {
                var _bits = (UInt32)(object)bits;
                var result = (UInt32)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt64Exts.Types.UInt64:
            {
                var _bits = (UInt64)(object)bits;
                var result = (UInt64)(object)(flags ?? default(TEnum)) & _bits;
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
                var _bits = (SByte)(object)bits;
                var result = (SByte)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int16Exts.Types.Int16:
            {
                var _bits = (Int16)(object)bits;
                var result = (Int16)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int32Exts.Types.Int32:
            {
                var _bits = (Int32)(object)bits;
                var result = (Int32)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int64Exts.Types.Int64:
            {
                var _bits = (Int64)(object)bits;
                var result = (Int64)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int128Exts.Types.Int128:
            {
                var _bits = (Int128)(object)bits;
                var result = (Int128)(object)(flags ?? default(TEnum)) & _bits;
                return result == _bits && result != 0;
            }
            default: break;
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
                var _bits = (Byte)(object)bits;
                var result = (Byte)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt16Exts.Types.UInt16:
            {
                var _bits = (UInt16)(object)bits;
                var result = (UInt16)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt32Exts.Types.UInt32:
            {
                var _bits = (UInt32)(object)bits;
                var result = (UInt32)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == UInt64Exts.Types.UInt64:
            {
                var _bits = (UInt64)(object)bits;
                var result = (UInt64)(object)(flags ?? default(TFlags)) & _bits;
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
                var _bits = (SByte)(object)bits;
                var result = (SByte)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int16Exts.Types.Int16:
            {
                var _bits = (Int16)(object)bits;
                var result = (Int16)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int32Exts.Types.Int32:
            {
                var _bits = (Int32)(object)bits;
                var result = (Int32)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int64Exts.Types.Int64:
            {
                var _bits = (Int64)(object)bits;
                var result = (Int64)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            case Type _t when _t == Int128Exts.Types.Int128:
            {
                var _bits = (Int128)(object)bits;
                var result = (Int128)(object)(flags ?? default(TFlags)) & _bits;
                return result == _bits && result != 0;
            }
            default: break;
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
            case Type _t when _t == ByteExts.Types.Byte: return (TStruct)(object)(value ? ((Byte)_f & ~(Byte)_b) : ((Byte)_f | (Byte)_b));
            case Type _t when _t == UInt16Exts.Types.UInt16: return (TStruct)(object)(value ? ((UInt16)_f & ~(UInt16)_b) : ((UInt16)_f | (UInt16)_b));
            case Type _t when _t == UInt32Exts.Types.UInt32: return (TStruct)(object)(value ? ((UInt32)_f & ~(UInt32)_b) : ((UInt32)_f | (UInt32)_b));
            case Type _t when _t == UInt64Exts.Types.UInt64: return (TStruct)(object)(value ? ((UInt64)_f & ~(UInt64)_b) : ((UInt64)_f | (UInt64)_b));
            case Type _t when _t == UInt128Exts.Types.UInt128: return (TStruct)(object)(value ? ((UInt128)_f & ~(UInt128)_b) : ((UInt128)_f | (UInt128)_b));
            case Type _t when _t == SByteExts.Types.SByte: return (TStruct)(object)(value ? ((SByte)_f & ~(SByte)_b) : ((SByte)_f | (SByte)_b));
            case Type _t when _t == Int16Exts.Types.Int16: return (TStruct)(object)(value ? ((Int16)_f & ~(Int16)_b) : ((Int16)_f | (Int16)_b));
            case Type _t when _t == Int32Exts.Types.Int32: return (TStruct)(object)(value ? ((Int32)_f & ~(Int32)_b) : ((Int32)_f | (Int32)_b));
            case Type _t when _t == Int64Exts.Types.Int64: return (TStruct)(object)(value ? ((Int64)_f & ~(Int64)_b) : ((Int64)_f | (Int64)_b));
            case Type _t when _t == Int128Exts.Types.Int128: return (TStruct)(object)(value ? ((Int128)_f & ~(Int128)_b) : ((Int128)_f | (Int128)_b));
            default: break;
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
            case Type _t when _t == ByteExts.Types.Byte: return (TResult)(object)(value ? ((Byte)_f & ~(Byte)_b) : ((Byte)_f | (Byte)_b));
            case Type _t when _t == UInt16Exts.Types.UInt16: return (TResult)(object)(value ? ((UInt16)_f & ~(UInt16)_b) : ((UInt16)_f | (UInt16)_b));
            case Type _t when _t == UInt32Exts.Types.UInt32: return (TResult)(object)(value ? ((UInt32)_f & ~(UInt32)_b) : ((UInt32)_f | (UInt32)_b));
            case Type _t when _t == UInt64Exts.Types.UInt64: return (TResult)(object)(value ? ((UInt64)_f & ~(UInt64)_b) : ((UInt64)_f | (UInt64)_b));
            case Type _t when _t == UInt128Exts.Types.UInt128: return (TResult)(object)(value ? ((UInt128)_f & ~(UInt128)_b) : ((UInt128)_f | (UInt128)_b));
            case Type _t when _t == SByteExts.Types.SByte: return (TResult)(object)(value ? ((SByte)_f & ~(SByte)_b) : ((SByte)_f | (SByte)_b));
            case Type _t when _t == Int16Exts.Types.Int16: return (TResult)(object)(value ? ((Int16)_f & ~(Int16)_b) : ((Int16)_f | (Int16)_b));
            case Type _t when _t == Int32Exts.Types.Int32: return (TResult)(object)(value ? ((Int32)_f & ~(Int32)_b) : ((Int32)_f | (Int32)_b));
            case Type _t when _t == Int64Exts.Types.Int64: return (TResult)(object)(value ? ((Int64)_f & ~(Int64)_b) : ((Int64)_f | (Int64)_b));
            case Type _t when _t == Int128Exts.Types.Int128: return (TResult)(object)(value ? ((Int128)_f & ~(Int128)_b) : ((Int128)_f | (Int128)_b));
            default: break;
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
            case Type _t when _t == ByteExts.Types.Byte: return (TEnum)(object)(value ? ((Byte)_f & ~(Byte)_b) : ((Byte)_f | (Byte)_b));
            case Type _t when _t == UInt16Exts.Types.UInt16: return (TEnum)(object)(value ? ((UInt16)_f & ~(UInt16)_b) : ((UInt16)_f | (UInt16)_b));
            case Type _t when _t == UInt32Exts.Types.UInt32: return (TEnum)(object)(value ? ((UInt32)_f & ~(UInt32)_b) : ((UInt32)_f | (UInt32)_b));
            case Type _t when _t == UInt64Exts.Types.UInt64: return (TEnum)(object)(value ? ((UInt64)_f & ~(UInt64)_b) : ((UInt64)_f | (UInt64)_b));
            case Type _t when _t == UInt128Exts.Types.UInt128: return (TEnum)(object)(value ? ((UInt128)_f & ~(UInt128)_b) : ((UInt128)_f | (UInt128)_b));
            case Type _t when _t == SByteExts.Types.SByte: return (TEnum)(object)(value ? ((SByte)_f & ~(SByte)_b) : ((SByte)_f | (SByte)_b));
            case Type _t when _t == Int16Exts.Types.Int16: return (TEnum)(object)(value ? ((Int16)_f & ~(Int16)_b) : ((Int16)_f | (Int16)_b));
            case Type _t when _t == Int32Exts.Types.Int32: return (TEnum)(object)(value ? ((Int32)_f & ~(Int32)_b) : ((Int32)_f | (Int32)_b));
            case Type _t when _t == Int64Exts.Types.Int64: return (TEnum)(object)(value ? ((Int64)_f & ~(Int64)_b) : ((Int64)_f | (Int64)_b));
            case Type _t when _t == Int128Exts.Types.Int128: return (TEnum)(object)(value ? ((Int128)_f & ~(Int128)_b) : ((Int128)_f | (Int128)_b));
            default: break;
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
            case Type _t when _t == ByteExts.Types.Byte: return (TResult)(object)(value ? ((Byte)_f & ~(Byte)_b) : ((Byte)_f | (Byte)_b));
            case Type _t when _t == UInt16Exts.Types.UInt16: return (TResult)(object)(value ? ((UInt16)_f & ~(UInt16)_b) : ((UInt16)_f | (UInt16)_b));
            case Type _t when _t == UInt32Exts.Types.UInt32: return (TResult)(object)(value ? ((UInt32)_f & ~(UInt32)_b) : ((UInt32)_f | (UInt32)_b));
            case Type _t when _t == UInt64Exts.Types.UInt64: return (TResult)(object)(value ? ((UInt64)_f & ~(UInt64)_b) : ((UInt64)_f | (UInt64)_b));
            case Type _t when _t == UInt128Exts.Types.UInt128: return (TResult)(object)(value ? ((UInt128)_f & ~(UInt128)_b) : ((UInt128)_f | (UInt128)_b));
            case Type _t when _t == SByteExts.Types.SByte: return (TResult)(object)(value ? ((SByte)_f & ~(SByte)_b) : ((SByte)_f | (SByte)_b));
            case Type _t when _t == Int16Exts.Types.Int16: return (TResult)(object)(value ? ((Int16)_f & ~(Int16)_b) : ((Int16)_f | (Int16)_b));
            case Type _t when _t == Int32Exts.Types.Int32: return (TResult)(object)(value ? ((Int32)_f & ~(Int32)_b) : ((Int32)_f | (Int32)_b));
            case Type _t when _t == Int64Exts.Types.Int64: return (TResult)(object)(value ? ((Int64)_f & ~(Int64)_b) : ((Int64)_f | (Int64)_b));
            case Type _t when _t == Int128Exts.Types.Int128: return (TResult)(object)(value ? ((Int128)_f & ~(Int128)_b) : ((Int128)_f | (Int128)_b));
            default: break;
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
            case Type _t when _t == ByteExts.Types.Byte: return (TResult)(object)(value ? ((Byte)_f & ~(Byte)_b) : ((Byte)_f | (Byte)_b));
            case Type _t when _t == UInt16Exts.Types.UInt16: return (TResult)(object)(value ? ((UInt16)_f & ~(UInt16)_b) : ((UInt16)_f | (UInt16)_b));
            case Type _t when _t == UInt32Exts.Types.UInt32: return (TResult)(object)(value ? ((UInt32)_f & ~(UInt32)_b) : ((UInt32)_f | (UInt32)_b));
            case Type _t when _t == UInt64Exts.Types.UInt64: return (TResult)(object)(value ? ((UInt64)_f & ~(UInt64)_b) : ((UInt64)_f | (UInt64)_b));
            case Type _t when _t == UInt128Exts.Types.UInt128: return (TResult)(object)(value ? ((UInt128)_f & ~(UInt128)_b) : ((UInt128)_f | (UInt128)_b));
            case Type _t when _t == SByteExts.Types.SByte: return (TResult)(object)(value ? ((SByte)_f & ~(SByte)_b) : ((SByte)_f | (SByte)_b));
            case Type _t when _t == Int16Exts.Types.Int16: return (TResult)(object)(value ? ((Int16)_f & ~(Int16)_b) : ((Int16)_f | (Int16)_b));
            case Type _t when _t == Int32Exts.Types.Int32: return (TResult)(object)(value ? ((Int32)_f & ~(Int32)_b) : ((Int32)_f | (Int32)_b));
            case Type _t when _t == Int64Exts.Types.Int64: return (TResult)(object)(value ? ((Int64)_f & ~(Int64)_b) : ((Int64)_f | (Int64)_b));
            case Type _t when _t == Int128Exts.Types.Int128: return (TResult)(object)(value ? ((Int128)_f & ~(Int128)_b) : ((Int128)_f | (Int128)_b));
            default: break;
        }

        return (TResult)(object)0;
    }
}