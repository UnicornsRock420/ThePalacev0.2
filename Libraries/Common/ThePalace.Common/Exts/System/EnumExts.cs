using System.Runtime.CompilerServices;

namespace System;

public static class EnumExts
{
    //static EnumExts()
    //{
    //}

    public static bool IsSet<TEnum>(this TEnum? bits, TEnum? flags)
        where TEnum : Enum
    {
        bits ??= default(TEnum);
        flags ??= default(TEnum);

        return flags.HasFlag(bits);
    }

    public static bool IsSet<TStruct>(this TStruct? bits, TStruct? flags)
        where TStruct : struct
    {
        bits ??= default(TStruct);
        flags ??= default(TStruct);

        return Unsafe.SizeOf<TStruct>() switch
        {
            1 => ((Func<bool>)(() =>
            {
                var _bits = (byte)(object)bits;
                var _flags = (byte)(object)flags;

                return (_flags & _bits) == _bits;
            }))(),
            2 => ((Func<bool>)(() =>
            {
                var _bits = (short)(object)bits;
                var _flags = (short)(object)flags;

                return (_flags & _bits) == _bits;
            }))(),
            4 => ((Func<bool>)(() =>
            {
                var _bits = (int)(object)bits;
                var _flags = (int)(object)flags;

                return (_flags & _bits) == _bits;
            }))(),
            8 => ((Func<bool>)(() =>
            {
                var _bits = (long)(object)bits;
                var _flags = (long)(object)flags;

                return (_flags & _bits) == _bits;
            }))(),
        };
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TEnum SetBit<TEnum>(this TEnum? bits, TEnum? flags, bool value)
        where TEnum : Enum
    {
        bits ??= default(TEnum);
        flags ??= default(TEnum);

        if (flags.HasFlag(bits) == value) return flags;

        return Unsafe.SizeOf<TEnum>() switch
        {
            1 => ((Func<TEnum>)(() =>
            {
                var _bits = (byte)(object)bits;
                var _flags = (byte)(object)flags;

                return (TEnum)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
            2 => ((Func<TEnum>)(() =>
            {
                var _bits = (short)(object)bits;
                var _flags = (short)(object)flags;

                return (TEnum)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
            4 => ((Func<TEnum>)(() =>
            {
                var _bits = (int)(object)bits;
                var _flags = (int)(object)flags;

                return (TEnum)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
            8 => ((Func<TEnum>)(() =>
            {
                var _bits = (long)(object)bits;
                var _flags = (long)(object)flags;

                return (TEnum)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
        };
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStruct SetBit<TStruct>(this TStruct? bits, TStruct? flags, bool value)
        where TStruct : struct
    {
        bits ??= default(TStruct);
        flags ??= default(TStruct);

        return Unsafe.SizeOf<TStruct>() switch
        {
            1 => ((Func<TStruct>)(() =>
            {
                var _bits = (byte)(object)bits;
                var _flags = (byte)(object)flags;

                return (TStruct)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
            2 => ((Func<TStruct>)(() =>
            {
                var _bits = (short)(object)bits;
                var _flags = (short)(object)flags;

                return (TStruct)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
            4 => ((Func<TStruct>)(() =>
            {
                var _bits = (int)(object)bits;
                var _flags = (int)(object)flags;

                return (TStruct)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
            8 => ((Func<TStruct>)(() =>
            {
                var _bits = (long)(object)bits;
                var _flags = (long)(object)flags;

                return (TStruct)(object)(value ? _flags | _bits : _flags & ~_bits);
            }))(),
        };
    }

    public static class Types
    {
        public static readonly Type Enum = typeof(Enum);
        public static readonly Type EnumArray = typeof(Enum[]);
        public static readonly Type EnumList = typeof(List<Enum>);
    }
}