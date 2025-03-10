using System.Text.RegularExpressions;
using Windows.Foundation.Metadata;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Helpers;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Common.Desktop.Constants;
using ThePalace.Common.Exts.System;
using ThePalace.Common.Exts.System.Collections.Generic;
using ThePalace.Common.Factories.System.Collections;
using ThePalace.Common.Helpers;

namespace ThePalace.Client.Desktop.Entities.UI;

public class MsgBubble : Disposable, IDisposable
{
    private const double _defaultRatio = 1.3;
    private const short _defaultOffset = 11;
    private const int _minWidth = 20;
    private const int _maxWidth = 150;

    private DateTime? _accessed;

    public MsgBubble(string text)
    {
        Parse(text);
    }

    public MsgBubble(string text, int duration)
    {
        Parse(text, duration);
    }

    public MsgBubble(short colorNbr, string text)
    {
        Colour = DesktopConstants.NbrToColor(colorNbr);
        Parse(text);
    }

    public MsgBubble(short colorNbr, string text, int duration)
    {
        Colour = DesktopConstants.NbrToColor(colorNbr);
        Parse(text, duration);
    }

    public MsgBubble(Color colour, string text)
    {
        Colour = colour;
        Parse(text);
    }

    public MsgBubble(Color colour, string text, int duration)
    {
        Colour = colour;
        Parse(text, duration);
    }

    public override void Dispose()
    {
        try
        {
            Image?.Dispose();
        }
        catch
        {
        }

        Text = null;
        MediaFilenames = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public Bitmap Image { get; private set; }

    public BubbleTypes Type { get; private set; } = BubbleTypes.Normal;
    public DateTime Created { get; } = DateTime.UtcNow;
    public DateTime Accessed => _accessed ??= DateTime.UtcNow;
    public bool Visible { get; private set; } = true;
    public Color Colour { get; } = Color.White;
    public Point Origin { get; private set; } = new(0, 0);
    public Size TextSize { get; set; } = new(0, 0);

    public int Duration { get; private set; }
    public string OriginalText { get; private set; }
    public string[] Text { get; private set; }
    public string[] MediaFilenames { get; private set; }

    #region Regex Matching & Formatting

    private IReadOnlyDictionary<Regex, Func<object[], string?>> _regexPatterns = new Dictionary<Regex, Func<object[], string?>>()
    {
        {
            RegexConstants.REGEX_HIDDEN, a =>
            {
                switch (a.Length)
                {
                    case < 1: throw new IndexOutOfRangeException("REGEX_HIDDEN[0]");
                    case < 2: throw new IndexOutOfRangeException("REGEX_HIDDEN[1]");
                }

                if (a[0] is not MsgBubble msgBubble) throw new ArgumentException(nameof(msgBubble));

                if (a[1] is not string text) throw new ArgumentException(nameof(text));

                msgBubble.Visible = false;

                return text;
            }
        },
        {
            RegexConstants.REGEX_BUBBLE_TYPE, a =>
            {
                switch (a.Length)
                {
                    case < 1: throw new IndexOutOfRangeException("REGEX_BUBBLE_TYPE[0]");
                    case < 2: throw new IndexOutOfRangeException("REGEX_BUBBLE_TYPE[1]");
                }

                if (a[0] is not MsgBubble msgBubble) throw new ArgumentException(nameof(msgBubble));

                if (a[1] is not string text) throw new ArgumentException(nameof(text));

                var match = RegexConstants.REGEX_BUBBLE_TYPE.Match(text);

                if (msgBubble.Type == BubbleTypes.Normal &&
                    _charBubbleTypeMappings.TryGetValue(match.Groups[1].Value[0], out var bubbleType))
                {
                    msgBubble.Type = bubbleType;
                }

                return match.Groups[2].Value?.Trim();
            }
        },
        {
            RegexConstants.REGEX_COORDINATES, a =>
            {
                switch (a.Length)
                {
                    case < 1: throw new IndexOutOfRangeException("REGEX_COORDINATES[0]");
                    case < 2: throw new IndexOutOfRangeException("REGEX_COORDINATES[1]");
                }

                if (a[0] is not MsgBubble msgBubble) throw new ArgumentException(nameof(msgBubble));

                if (a[1] is not string text) throw new ArgumentException(nameof(text));

                var match = RegexConstants.REGEX_COORDINATES.Match(text);

                var x = Convert.ToInt32(match.Groups[1].Value);
                var y = Convert.ToInt32(match.Groups[2].Value);
                msgBubble.Origin = new Point(x, y);

                return match.Groups[3].Value?.Trim();
            }
        },
        {
            RegexConstants.REGEX_SOUND, a =>
            {
                switch (a.Length)
                {
                    case < 1: throw new IndexOutOfRangeException("REGEX_SOUND[0]");
                    case < 2: throw new IndexOutOfRangeException("REGEX_SOUND[1]");
                }

                if (a[0] is not MsgBubble msgBubble) throw new ArgumentException(nameof(msgBubble));

                if (a[1] is not string text) throw new ArgumentException(nameof(text));

                var mediaFilenames = new List<string>();
                var match = (Match?)null;

                while (RegexConstants.REGEX_SOUND.IsMatch(text))
                {
                    match = RegexConstants.REGEX_SOUND.Match(text);
                    if (match == null) break;

                    var fileName = match.Groups[1].Value?.Trim();
                    if (!string.IsNullOrWhiteSpace(fileName))
                        mediaFilenames.Add(fileName);

                    text = match.Groups[2].Value?.Trim();
                    if (string.IsNullOrWhiteSpace(text)) break;
                }

                if (mediaFilenames.Count > 0)
                    msgBubble.MediaFilenames = mediaFilenames.ToArray();

                return text;
            }
        },
    }.AsReadOnly();

    private static readonly Regex[] CONST_regexSequence =
    [
        RegexConstants.REGEX_HIDDEN,
        RegexConstants.REGEX_BUBBLE_TYPE,
        RegexConstants.REGEX_COORDINATES,
        RegexConstants.REGEX_BUBBLE_TYPE,
        RegexConstants.REGEX_SOUND,
        RegexConstants.REGEX_BUBBLE_TYPE
    ];

    #endregion

    private void Parse(string text, int duration = 0)
    {
        OriginalText = text;

        foreach (var step in CONST_regexSequence)
        {
            if (step.IsMatch(text) &&
                _regexPatterns.TryGetValue(step, out var cb))
            {
                text = cb([this, text]);

                if (!Visible) break;
            }
        }

        #region Word(s) & Line Formatting

        var words = Common.Constants.RegexConstants.REGEX_WHITESPACE_SINGLELINE.Split(text).ToList();
        var lines = new List<string>();
        var line = new List<string>();

        while (words.Count > 0)
        {
            var newLine = line.Join(" ", words[0]);
            var newLineSize = TextRenderer.MeasureText(newLine,
                new Font(DesktopConstants.Font.NAME, DesktopConstants.Font.HEIGHT));

            if (newLineSize.Width < _maxWidth)
                line.Enqueue(words.Dequeue());

            if (newLineSize.Width < _maxWidth &&
                words.Count >= 1) continue;

            newLine = line.Join(" ");
            newLineSize = TextRenderer.MeasureText(newLine,
                new Font(DesktopConstants.Font.NAME, DesktopConstants.Font.HEIGHT));

            if (newLineSize.Width > TextSize.Width)
                TextSize = new Size(newLineSize.Width, TextSize.Height);

            lines.Add(newLine);
            line.Clear();
        }

        Text = lines.ToArray();
        Duration = duration > 0 ? duration : Text.Join(" ").ToCharArray().Length * 500;

        TextSize = new Size(
            TextSize.Width, // Width += -28; // 16;
            Text.Length * (DesktopConstants.Font.HEIGHT - 5) + 2);

        #endregion
    }

    private delegate void RenderMethod(MsgBubble msgBubble, GraphicsHelper helper, int w, int h, short x, short y);

    private static readonly IReadOnlyDictionary<BubbleTypes, RenderMethod> _bubbleTypeMethodMappings = new Dictionary<BubbleTypes, RenderMethod>()
    {
        { BubbleTypes.Normal, Render_Normal },
        { BubbleTypes.Shout, Render_Shout },
        { BubbleTypes.Sticky, Render_Sticky },
        { BubbleTypes.Thought, Render_Thought2 }
    }.AsReadOnly();

    private static readonly IReadOnlyDictionary<char, BubbleTypes> _charBubbleTypeMappings = new Dictionary<char, BubbleTypes>()
    {
        { '!', BubbleTypes.Shout },
        { ':', BubbleTypes.Thought },
        { '^', BubbleTypes.Sticky }
    }.AsReadOnly();

    private static void Render_Normal(MsgBubble msgBubble, GraphicsHelper helper, int w, int h, short x, short y)
    {
        var px = (short)msgBubble.Origin.X;
        var py = (short)msgBubble.Origin.Y;
        var r = (short)(x + w);
        var b = (short)(y + h);
        var con1 = (short)0;
        var con2 = (short)0;
        var radius = 18;

        if (py < y || py > y + h)
        {
            con1 = (short)Math.Min(Math.Max(x + radius, px - 10), r - radius - 20);
            con2 = (short)Math.Min(Math.Max(x + radius + 20, px + 10), r - radius);
        }
        else
        {
            con1 = (short)Math.Min(Math.Max(y + radius, py - 10), b - radius - 20);
            con2 = (short)Math.Min(Math.Max(y + radius + 20, py + 10), b - radius);
        }

        var dir = (short)0;
        if (py < y) dir = 2;
        if (py > y) dir = 3;
        if (px < x && py >= y && py <= b) dir = 0;
        if (px > x && py >= y && py <= b) dir = 1;
        if (px >= x && px <= r && py >= y && py <= b) dir = -1;

        px = 0;
        py = 0;

        helper.BeginPath();

        helper.MoveTo(x + radius, y);

        if (dir == 2)
        {
            helper.LineTo(con1, y);
            helper.LineTo(px, py);
            helper.LineTo(con2, y);
        }

        helper.LineTo(r - radius, y);
        helper.QuadraticCurveTo(r, y, r, y + radius);

        if (dir == 1)
        {
            helper.LineTo(r, con1);
            helper.LineTo(px, py);
            helper.LineTo(r, con2);
        }

        helper.LineTo(r, b - radius);
        helper.QuadraticCurveTo(r, b, r - radius, b);

        if (dir == 3)
        {
            helper.LineTo(con2, b);
            helper.LineTo(px, py);
            helper.LineTo(con1, b);
        }

        helper.LineTo(x + radius, b);
        helper.QuadraticCurveTo(x, b, x, b - radius);

        if (dir == 0)
        {
            helper.LineTo(x, con2);
            helper.LineTo(px, py);
            helper.LineTo(x, con1);
        }

        helper.LineTo(x, y + radius);
        helper.QuadraticCurveTo(x, y, x + radius, y);

        helper.Fill();
        helper.Stroke();
    }

    private static void Render_Shout(MsgBubble msgBubble, GraphicsHelper helper, int w, int h, short x, short y)
    {
        var SpikeDisplace = (short)20;
        var SpikeHeight = (short)16;
        var SpikeWidth = (short)12;
        var r = (short)(x + w);
        var b = (short)(y + h);

        var rect =
            new Rectangle(
                x, y,
                r, b);

        var hp = (rect.Width - rect.X) / SpikeWidth;
        if (hp < 2) hp = 2;
        var vp = (rect.Height - rect.Y) / SpikeHeight;
        if (vp < 2) vp = 2;
        var hw = (rect.Width - rect.X) / hp;
        var vw = (rect.Height - rect.Y) / vp;

        var px = (short)0;
        var py = (short)0;
        var _px = (short)0;
        var _py = (short)0;

        helper.BeginPath();

        helper.MoveTo((short)rect.X, (short)rect.Y);

        for (var tx = 0; tx < hp; ++tx)
        {
            px = (short)(rect.X + tx * hw + hw / 2 + tx * SpikeDisplace / (hp - 1) - SpikeDisplace / 2);
            py = (short)(rect.Y - SpikeHeight);

            _px = (short)(rect.X + tx * hw + hw);
            _py = (short)rect.Y;

            helper.LineTo(px, py);
            helper.LineTo(_px, _py);
        }

        for (var ty = 0; ty < vp; ++ty)
        {
            px = (short)(rect.Width + SpikeHeight);
            py = (short)(rect.Y + ty * vw + vw / 2 + ty * SpikeDisplace / (vp - 1) - SpikeDisplace / 2);

            _px = (short)rect.Width;
            _py = (short)(rect.Y + ty * vw + vw);

            helper.LineTo(px, py);
            helper.LineTo(_px, _py);
        }

        for (var tx = 0; tx < hp; ++tx)
        {
            px = (short)(rect.Width - tx * hw - hw / 2 + (hp - 1 - tx) * SpikeDisplace / (hp - 1) - SpikeDisplace / 2);
            py = (short)(rect.Height + SpikeHeight);

            _px = (short)(rect.Width - tx * hw - hw);
            _py = (short)rect.Height;

            helper.LineTo(px, py);
            helper.LineTo(_px, _py);
        }

        for (var ty = 0; ty < vp; ++ty)
        {
            px = (short)(rect.X - SpikeHeight);
            py = (short)(rect.Height - ty * vw - vw / 2 + (vp - 1 - ty) * SpikeDisplace / (vp - 1) - SpikeDisplace / 2);

            _px = (short)rect.X;
            _py = (short)(rect.Height - ty * vw - vw);

            helper.LineTo(px, py);
            helper.LineTo(_px, _py);
        }

        helper.LineTo((short)rect.X, (short)rect.Y);

        helper.Fill();
        helper.Stroke();
    }

    private static void Render_Sticky(MsgBubble msgBubble, GraphicsHelper helper, int w, int h, short x, short y)
    {
        var r = (short)(x + w);
        var b = (short)(y + h);
        var radius = 1;

        helper.BeginPath();

        helper.MoveTo(x + radius, y);

        helper.LineTo(r - radius, y);
        helper.QuadraticCurveTo(r, y, r, y + radius);
        helper.LineTo(r, y + h - radius);
        helper.QuadraticCurveTo(r, b, r - radius, b);
        helper.LineTo(x + radius, b);
        helper.QuadraticCurveTo(x, b, x, b - radius);
        helper.LineTo(x, y + radius);
        helper.QuadraticCurveTo(x, y, x + radius, y);

        helper.Fill();
        helper.Stroke();
    }

    [Deprecated(null, DeprecationType.Deprecate, 0)]
    private static void Render_Thought(MsgBubble msgBubble, GraphicsHelper helper, int w, int h, short x, short y)
    {
        var r = (short)(x + w);
        var b = (short)(y + h);

        var px = (short)0;
        var py = (short)0;
        var radius = 4;

        var dir = (short)0;
        if (py < y) dir = 2;
        if (py > y) dir = 3;
        if (px < x && py >= y && py <= b) dir = 0;
        if (px > x && py >= y && py <= b) dir = 1;
        if (px >= x && px <= r && py >= y && py <= b) dir = -1;

        dir = (short)RndGenerator.Next(0, 4);
        switch (dir)
        {
            case 2:
                radius = 4;
                px = (short)(x - radius * 2.5);
                py = (short)(y - radius);

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                radius = 8;
                px = (short)(x - radius);
                py = y;

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                break;
            case 1:
                radius = 4;
                px = (short)(x - 4);
                py = (short)(b + radius * 2.5);

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                radius = 8;
                px = x;
                py = (short)(b + 2);

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                break;
            case 3:
                radius = 4;
                px = (short)(r + 4);
                py = (short)(b + radius * 2.5);

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                radius = 8;
                px = (short)(r - 4);
                py = (short)(b + 2);

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                break;
            case 0:
                radius = 4;
                px = (short)(r + radius * 2.5);
                py = (short)(y - radius);

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                radius = 8;
                px = r;
                py = y;

                helper.BeginPath();
                helper.DrawEllipse(
                    new Rectangle(
                        px, py,
                        radius,
                        radius));
                helper.Fill();
                helper.Stroke();

                break;
        }

        radius = 24;
        helper.BeginPath();

        helper.MoveTo(x + radius, y);

        helper.LineTo(r - radius, y);
        helper.QuadraticCurveTo(r, y, r, y + radius);
        helper.LineTo(r, y + h - radius);
        helper.QuadraticCurveTo(r, b, r - radius, b);
        helper.LineTo(x + radius, b);
        helper.QuadraticCurveTo(x, b, x, b - radius);
        helper.LineTo(x, y + radius);
        helper.QuadraticCurveTo(x, y, x + radius, y);

        helper.Fill();
        helper.Stroke();
    }

    private static void Render_Thought2(MsgBubble msgBubble, GraphicsHelper helper, int w, int h, short x, short y)
    {
        var r = (short)(x + w);
        var b = (short)(y + h);
        //var px = (short)0;
        //var py = (short)0;

        var xMax = msgBubble.TextSize.Width / 14;
        var yMax = msgBubble.TextSize.Height / 13;
        var radius = (short)0;
        var rMin = 12;
        var rMax = 15;

        helper.BeginPath();

        for (var j = 0; j <= yMax; j++)
        {
            radius = (short)RndGenerator.Next(rMin, rMax);

            var ofst = rMax - radius + 1;

            helper.DrawArc(
                new Rectangle(
                    x - ofst,
                    y,
                    radius,
                    radius),
                90F,
                180F);

            if (j < yMax)
                y += radius;
        }

        helper.DrawArc(
            new Rectangle(
                x,
                y,
                radius,
                radius),
            0F,
            180F);

        for (var j = 0; j <= xMax; j++)
        {
            radius = (short)RndGenerator.Next(rMin, rMax);

            var ofst = rMax - radius + 1;

            helper.DrawArc(
                new Rectangle(
                    x,
                    y + ofst,
                    radius,
                    radius),
                0F,
                180F);

            if (j < xMax)
                x += radius;
        }

        helper.DrawArc(
            new Rectangle(
                x,
                y,
                radius,
                radius),
            270F,
            180F);

        for (var j = 0; j <= yMax; j++)
        {
            radius = (short)RndGenerator.Next(rMin, rMax);

            var ofst = rMax - radius + 1;

            helper.DrawArc(
                new Rectangle(
                    x + ofst,
                    y,
                    radius,
                    radius),
                270F,
                180F);

            if (j < yMax)
                y -= radius;
        }

        helper.DrawArc(
            new Rectangle(
                x,
                y,
                radius,
                radius),
            180F,
            180F);

        for (var j = 0; j <= xMax; j++)
        {
            radius = (short)RndGenerator.Next(rMin, rMax);

            var ofst = rMax - radius + 1;

            helper.DrawArc(
                new Rectangle(
                    x,
                    y - ofst,
                    radius,
                    radius),
                180F,
                180F);

            if (j < xMax)
                x -= radius;
        }

        //helper.DrawArc(
        //    new Rectangle(
        //        x,
        //        y,
        //        radius,
        //        radius),
        //        90F,
        //        180F);

        helper.Fill();
        helper.Stroke();
    }

    public async Task<Bitmap?> Render(IDesktopSessionState sessionState) =>
        await Task.Factory.StartNew(() =>
        {
            if (Text.Length < 1) return null;

            var px = (short)this.Origin.X;
            var py = (short)this.Origin.Y;

            var offsetX = _defaultOffset;
            var x = (short)(Origin.X + offsetX);
            var w = new[] { _minWidth, TextSize.Width }.Max();
            var r = (short)(x + w);

            var offsetY = _defaultOffset;
            var y = (short)(Origin.Y + offsetY);
            var h = TextSize.Height;
            var b = (short)(y + h);

            if (sessionState.ScreenWidth < r)
            {
                offsetX = -_defaultOffset;

                x -= (short)(w * _defaultRatio);
                r = (short)(x + w);
            }

            if (sessionState.ScreenHeight < b)
            {
                offsetY = -_defaultOffset;

                y -= (short)(h * _defaultRatio);
                b = (short)(y + h);
            }

            var imagePadding = 50;
            Image = new Bitmap(TextSize.Width + imagePadding, TextSize.Height + imagePadding);
            if (Image == null) throw new OutOfMemoryException(nameof(MsgBubble) + "." + nameof(Render) + "." + nameof(Image));

            using (var g = Graphics.FromImage(Image))
            using (var colourBrush = new SolidBrush(Colour))
            using (var colourPen = new Pen(colourBrush, 2))
            {
                var helper = new GraphicsHelper(
                    g,
                    new Font(
                        DesktopConstants.Font.NAME,
                        DesktopConstants.Font.HEIGHT),
                    colourPen,
                    colourBrush);

                if (_bubbleTypeMethodMappings.TryGetValue(Type, out var ptr))
                {
                    ptr(this, helper, w, h, x, y);
                }

                helper.SetBrush(Brushes.Black);
                helper.BeginPath();

                var deltaX = (short)6;
                var deltaY1 = (short)6;
                var deltaY2 = (short)16;

                for (var k = 0; k < Text.Length; k++)
                {
                    helper.DrawText(Text[k], offsetX + x + deltaX, Math.Abs(offsetY > 0 ? offsetY : _defaultOffset) + y - deltaY1 + k * deltaY2);
                }

                helper.Fill();
                helper.Stroke();

                return Image;
            }
        });
}