using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Exts
{
    public static class AttributeExts
    {
        public static class Types
        {
            public static readonly Type MnemonicAttribute = typeof(MnemonicAttribute);
            public static readonly Type BitSizeAttribute = typeof(BitSizeAttribute);
            public static readonly Type ByteSizeAttribute = typeof(ByteSizeAttribute);
        }

        public static string? GetMnemonic<T>()
        {
            var attribs = new List<MnemonicAttribute>();
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    attribs.AddRange(_t
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>() ?? []);
                    break;
                case Type _e when _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return null;

                    attribs.AddRange(_e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>() ?? []);
                    break;
            }

            if (attribs.Count < 1) return null;

            return attribs
                .Select(a => a.Mnemonic)
                .FirstOrDefault();
        }

        public static uint GetHexValue<T>()
        {
            var attribs = new List<MnemonicAttribute>();
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    attribs.AddRange(_t
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>() ?? []);
                    break;
                case Type _e when _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return 0;

                    attribs.AddRange(_e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.MnemonicAttribute, false)
                        ?.Cast<MnemonicAttribute>() ?? []);
                    break;
            }

            if (attribs.Count < 1) return 0;

            return attribs
                .Select(a => a.HexValue)
                .FirstOrDefault();
        }

        public static int GetBitSize<T>()
        {
            var byteAttribs = new List<ByteSizeAttribute>();
            var bitAttribs = new List<BitSizeAttribute>();
            var type = typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    byteAttribs.AddRange(_t
                        ?.GetCustomAttributes(Types.ByteSizeAttribute, false)
                        ?.Cast<ByteSizeAttribute>() ?? []);
                    bitAttribs.AddRange(_t
                        ?.GetCustomAttributes(Types.ByteSizeAttribute, false)
                        ?.Cast<BitSizeAttribute>() ?? []);
                    break;

                //?.Select(a => a.ByteSize * 8)
                //?.FirstOrDefault() ?? _t
                //?.GetCustomAttributes(Types.BitSizeAttribute, false)
                //?.Cast<BitSizeAttribute>()
                //?.Select(a => a.BitSize)
                //?.FirstOrDefault() ?? 0;
                case Type _e when _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return 0;

                    byteAttribs.AddRange(_e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.ByteSizeAttribute, false)
                        ?.Cast<ByteSizeAttribute>() ?? []);
                    bitAttribs.AddRange(_e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.ByteSizeAttribute, false)
                        ?.Cast<BitSizeAttribute>() ?? []);
                    break;
            }

            if ((byteAttribs.Count + bitAttribs.Count) < 1) return 0;

            return byteAttribs.Select(a => a.ByteSize * 8)
                .Union(bitAttribs.Select(a => a.BitSize))
                .FirstOrDefault();
        }

        public static int GetByteSize<T>() => GetByteSize(typeof(T));

        public static int GetByteSize(this Type type)
        {
            var attribs = new List<ByteSizeAttribute>();
            switch (type)
            {
                case Type _t when _t is Type:
                    attribs.AddRange(_t
                        ?.GetCustomAttributes(Types.ByteSizeAttribute, false)
                        ?.Cast<ByteSizeAttribute>() ?? []);
                    break;
                case Type _e when _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return 0;

                    attribs.AddRange(_e
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.ByteSizeAttribute, false)
                        ?.Cast<ByteSizeAttribute>() ?? []);
                    break;
            }

            if (attribs.Count < 1) return 0;

            return attribs
                .Select(a => a.ByteSize)
                .FirstOrDefault();
        }

        //static AttributeExts() { }
    }
}