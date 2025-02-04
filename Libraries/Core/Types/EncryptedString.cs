using System.Runtime.InteropServices;
using ThePalace.Core.Enums;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces;
using uint8 = System.Byte;

namespace ThePalace.Core.Types
{
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct EncryptedString : IString, IComparable
    {
        public static explicit operator uint8[](EncryptedString p) => p.Value ?? [];
        public static explicit operator char[](EncryptedString p) => p.ToString()?.ToCharArray() ?? [];
        public static explicit operator string(EncryptedString p) => p.ToString() ?? string.Empty;
        public static explicit operator EncryptedString(uint8[] v) => new(v);
        public static explicit operator EncryptedString(char[] v) => new(v);
        public static explicit operator EncryptedString(string v) => new(v);

        #region cStr
        public EncryptedString(uint8[] value, EncryptedStringOptions opts = EncryptedStringOptions.None)
        {
            if (value != null &&
                value.Length > 0)
            {
                this._value = value;

                Options(opts);
            }
        }
        public EncryptedString(char[] value, EncryptedStringOptions opts = EncryptedStringOptions.None)
        {
            if (value != null &&
                value.Length > 0)
            {
                this._value = value.GetBytes();

                Options(opts);
            }
        }
        public EncryptedString(string value, EncryptedStringOptions opts = EncryptedStringOptions.None)
        {
            if (value != null &&
                value.Length > 0)
            {
                if (opts.IsBit<EncryptedStringOptions, EncryptedStringOptions>(EncryptedStringOptions.FromHex))
                    this._value = value.FromHex();
                else
                    this._value = value.GetBytes();

                Options(opts);
            }
        }
        #endregion

        #region Fields & Properties
        public readonly uint8 Length => (uint8)(_value?.Length ?? 0);

        private uint8[]? _value = [];
        public uint8[] Value
        {
            readonly get
            {
                return _value ?? [];
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _value = value;
            }
        }
        #endregion

        #region Methods
        public void Options(EncryptedStringOptions opts = EncryptedStringOptions.None)
        {
            if (opts.IsBit<EncryptedStringOptions, EncryptedStringOptions>(EncryptedStringOptions.EncryptString))
            {
                this._value = this._value.EncryptBytes();
            }
            else if (opts.IsBit<EncryptedStringOptions, EncryptedStringOptions>(EncryptedStringOptions.DecryptString))
            {
                this._value = this._value.DecryptBytes();
            }
        }

        public string? ToString(EncryptedStringOptions opts = EncryptedStringOptions.DecryptString)
        {
            var result = (string?)null;

            if (opts.IsBit<EncryptedStringOptions, EncryptedStringOptions>(EncryptedStringOptions.DecryptString))
            {
                result = this._value.DecryptString();
            }
            else
            {
                result = this._value.GetString();
            }

            if (opts.IsBit<EncryptedStringOptions, EncryptedStringOptions>(EncryptedStringOptions.ToHex))
            {
                result = result.ToHex();
            }

            return result;
        }

        public readonly TypeCode GetTypeCode() => TypeCode.Byte;

        public readonly object ToType(Type conversionType, IFormatProvider? provider) => ByteExts.Types.ByteArray;

        public readonly int CompareTo(object? obj) => 0;

        public readonly bool ToBoolean(IFormatProvider? provider) => true;

        public DateTime ToDateTime(IFormatProvider? provider) => DateTime.UnixEpoch;

        public byte ToByte(IFormatProvider? provider) => (byte)(_value?.FirstOrDefault() ?? 0);

        public char ToChar(IFormatProvider? provider) => (char)(_value?.FirstOrDefault() ?? 0);

        public decimal ToDecimal(IFormatProvider? provider) => (decimal)(_value?.FirstOrDefault() ?? 0);

        public double ToDouble(IFormatProvider? provider) => (double)(_value?.FirstOrDefault() ?? 0);

        public short ToInt16(IFormatProvider? provider) => (short)(_value?.FirstOrDefault() ?? 0);

        public int ToInt32(IFormatProvider? provider) => (int)(_value?.FirstOrDefault() ?? 0);

        public long ToInt64(IFormatProvider? provider) => (long)(_value?.FirstOrDefault() ?? 0);

        public sbyte ToSByte(IFormatProvider? provider) => (sbyte)(_value?.FirstOrDefault() ?? 0);

        public float ToSingle(IFormatProvider? provider) => (char)(_value?.FirstOrDefault() ?? 0);

        public ushort ToUInt16(IFormatProvider? provider) => (ushort)(_value?.FirstOrDefault() ?? 0);

        public uint ToUInt32(IFormatProvider? provider) => (uint)(_value?.FirstOrDefault() ?? 0);

        public ulong ToUInt64(IFormatProvider? provider) => (ulong)(_value?.FirstOrDefault() ?? 0);
        #endregion
    }
}