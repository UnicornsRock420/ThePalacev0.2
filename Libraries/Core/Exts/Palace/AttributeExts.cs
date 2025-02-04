using ThePalace.Core.Attributes;

namespace ThePalace.Core.Exts.Palace
{
    public static class AttributeExts
    {
        public static class Types
        {
            public static readonly Type MnemonicAttribute = typeof(MnemonicAttribute);
            public static readonly Type BitWidthAttribute = typeof(BitSizeAttribute);
            public static readonly Type ByteWidthAttribute = typeof(ByteSizeAttribute);
        }

        public static string? GetMnemonic<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    return _t
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>()
                        ?.Select(a => a.Mnemonic)
                        ?.FirstOrDefault();
                case Type _e when _e is Enum || _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return null;

                    return type
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>()
                        ?.Select(a => a.Mnemonic)
                        ?.FirstOrDefault() ?? key;
            }

            return null;
        }

        public static uint GetHexValue<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    return _t
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>()
                        ?.Select(a => a.HexValue)
                        ?.FirstOrDefault() ?? 0;
                case Type _e when _e is Enum || _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return 0;

                    return _e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>()
                        ?.Select(a => a.HexValue)
                        ?.FirstOrDefault() ?? 0;
            }

            return 0;
        }

        public static int GetBitSize<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    return _t
                        ?.GetCustomAttributes(Types.ByteWidthAttribute, false)
                        ?.Cast<ByteSizeAttribute>()
                        ?.Select(a => a.ByteSize * 8)
                        ?.FirstOrDefault() ?? _t
                        ?.GetCustomAttributes(Types.BitWidthAttribute, false)
                        ?.Cast<BitSizeAttribute>()
                        ?.Select(a => a.BitSize)
                        ?.FirstOrDefault() ?? 0;
                case Type _e when _e is Enum || _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return 0;

                    return _e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.ByteWidthAttribute, false)
                        ?.Cast<ByteSizeAttribute>()
                        ?.Select(a => a.ByteSize * 8)
                        ?.FirstOrDefault() ?? type
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.BitWidthAttribute, false)
                        ?.Cast<BitSizeAttribute>()
                        ?.Select(a => a.BitSize)
                        ?.FirstOrDefault() ?? 0;
            }

            return 0;
        }

        public static int GetByteSize<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    return _t
                        ?.GetCustomAttributes(Types.ByteWidthAttribute, false)
                        ?.Cast<ByteSizeAttribute>()
                        ?.Select(a => a.ByteSize)
                        ?.FirstOrDefault() ?? 0;
                case Type _e when _e is Enum || _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return 0;

                    return type
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.ByteWidthAttribute, false)
                        ?.Cast<ByteSizeAttribute>()
                        ?.Select(a => a.ByteSize)
                        ?.FirstOrDefault() ?? 0;
            }

            return 0;
        }

        //static AttributeExts() { }
    }
}