namespace ThePalace.Common.Desktop.Constants;

public class DesktopConstants
{
    public const int MaxNbrFaces = 13;
    public const int MaxNbrColors = 16;

    public static readonly uint[] SmileyColours =
    [
        0xFFFF0000,
        0xFFFF5F00,
        0xFFFFBF00,
        0xFFDFFF00,
        0xFF7FFF00,
        0xFF1FFF00,
        0xFF00FF3F,
        0xFF00FF9F,
        0xFF00FFFF,
        0xFF009FFF,
        0xFF003FFF,
        0xFF1F00FF,
        0xFF7F00FF,
        0xFFDF00FF,
        0xFFFF00BF,
        0xFFFF005F
    ];

    public static Color NbrToColor(short colorNbr)
    {
        return Color.FromArgb((int)SmileyColours[colorNbr % MaxNbrColors]);
    }

    public static class Font
    {
        public const string NAME = "Arial";
        public const int HEIGHT = 14;
    }

    public static class AspectRatio
    {
        public static class StandardDef
        {
            public const int Multiplier = 128;
            public static readonly Size Ratio = new(4, 3);
            public static readonly Size Default = new(Ratio.Width * Multiplier, Ratio.Height * Multiplier);
        }

        public static class WidescreenDef
        {
            public const int Multiplier = 96;
            public static readonly Size Ratio = new(16, 9);
            public static readonly Size Default = new(Ratio.Width * Multiplier, Ratio.Height * Multiplier);
        }
    }
}