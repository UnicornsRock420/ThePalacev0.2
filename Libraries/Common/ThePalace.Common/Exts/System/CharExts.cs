namespace ThePalace.Common.Exts.System;

public static class CharExts
{
    //static CharExts() { }

    public static string GetString(this IEnumerable<char> data, int limit = 0, int offset = 0)
    {
        return data.ToArray().GetString(limit, offset);
    }

    public static string GetString(this char[] data, int limit = 0, int offset = 0)
    {
        if ((data?.Length ?? 0) < 1) return string.Empty;

        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), nameof(limit) + " cannot be less than 0");
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset), nameof(offset) + " cannot be less than 0");
        if (offset + limit > data.Length)
            throw new IndexOutOfRangeException(nameof(offset) + " is out of bounds of the source");

        if (limit < 1 || limit > data.Length)
            limit = data.Length;

        return string.Concat(data
            .Skip(offset)
            .Take(limit));
    }

    public static byte[] GetBytes(this IEnumerable<char> data, int limit = 0, int offset = 0)
    {
        return data.ToArray().GetBytes(limit, offset);
    }

    public static byte[] GetBytes(this char[] data, int limit = 0, int offset = 0)
    {
        if ((data?.Length ?? 0) < 1) return [];

        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), nameof(limit) + " cannot be less than 0");
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset), nameof(offset) + " cannot be less than 0");
        if (offset + limit > data.Length)
            throw new IndexOutOfRangeException(nameof(offset) + " is out of bounds of the source");

        if (limit < 1 || limit > data.Length)
            limit = data.Length;

        return data
            .Skip(offset)
            .Take(limit)
            .Select(b => (byte)b)
            .ToArray();
    }

    public static class Types
    {
        public static readonly Type Char = typeof(char);
        public static readonly Type CharArray = typeof(char[]);
        public static readonly Type CharList = typeof(List<char>);
    }
}