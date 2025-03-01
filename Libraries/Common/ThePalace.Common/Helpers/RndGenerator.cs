namespace ThePalace.Common.Helpers;

public static class RndGenerator
{
    private const uint _KvTtl = 3;
    private static Random _RndGenerator;
    private static DateTime _UpdateDate;

    private static void CheckTTL()
    {
        if (_RndGenerator == null || DateTime.UtcNow.Subtract(_UpdateDate).Minutes > _KvTtl)
        {
            try
            {
                _RndGenerator = new Random();
                _UpdateDate = DateTime.UtcNow;
            }
            catch
            {
            }
        }
    }

    public static int Next(int minValue, int maxValue)
    {
        if (maxValue < 1)
        {
            return 0;
        }

        CheckTTL();

        return _RndGenerator.Next(minValue, maxValue);
    }

    public static int Next(int maxValue)
    {
        if (maxValue < 1)
        {
            return 0;
        }

        CheckTTL();

        return _RndGenerator.Next(maxValue);
    }

    public static int Next()
    {
        CheckTTL();

        return _RndGenerator.Next();
    }

    public static uint Next(uint minValue, uint maxValue)
    {
        if (maxValue < 1)
        {
            return 0;
        }

        CheckTTL();

        return (uint)_RndGenerator.Next((int)minValue, (int)maxValue);
    }

    public static uint Next(uint maxValue)
    {
        if (maxValue < 1)
        {
            return 0;
        }

        CheckTTL();

        return (uint)_RndGenerator.Next((int)maxValue);
    }

    public static byte[] NextBytes(int size)
    {
        if (size < 1)
        {
            size = 1;
        }

        CheckTTL();

        var result = new byte[size];

        _RndGenerator.NextBytes(result);

        return result;
    }

    public static double NextDouble()
    {
        CheckTTL();

        return _RndGenerator.NextDouble();
    }
}