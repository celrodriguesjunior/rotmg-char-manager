using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RotmgManager.UI;

/// <summary>
/// Provides simple generated icons for classes so the editor can show a visual cue without bundling external assets.
/// </summary>
public sealed class ClassIconProvider : IDisposable
{
    private readonly Dictionary<string, Image> cache = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Color> palette = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Wizard"] = Color.FromArgb(108, 52, 131),
        ["Knight"] = Color.FromArgb(52, 73, 94),
        ["Priest"] = Color.FromArgb(243, 156, 18),
        ["Assassin"] = Color.FromArgb(192, 57, 43),
        ["Huntress"] = Color.FromArgb(39, 174, 96),
        ["Necromancer"] = Color.FromArgb(99, 57, 116)
    };

    public Image GetIcon(string className)
    {
        if (string.IsNullOrWhiteSpace(className))
        {
            className = "Unknown";
        }

        if (cache.TryGetValue(className, out Image? existing))
        {
            return existing;
        }

        var created = CreateIcon(className);
        cache[className] = created;
        return created;
    }

    private Image CreateIcon(string className)
    {
        const int size = 160;
        const string dir = "C:\\dev\\meuu\\rotmg-char-manager\\img";

        Bitmap bitmap;

        if (className != "Knight")
        {
            bitmap = new Bitmap($"{dir}\\{className.ToLower()}.png");
            bitmap = new Bitmap(bitmap, size, size);

        }
        else
        {
            bitmap = new Bitmap(size, size);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.Black);

            using (var brush = new LinearGradientBrush(
                       new Rectangle(0, 0, size, size),
                       GetColorForClass(className),
                       Color.FromArgb(30, Color.White),
                       45f))
            {
                graphics.FillRectangle(brush, 0, 0, size, size);
            }

            using (var borderPen = new Pen(Color.FromArgb(150, Color.Black), 4))
            {
                graphics.DrawRectangle(borderPen, 2, 2, size - 4, size - 4);
            }

            string text = GetDisplayText(className);
            using var font = new Font("Segoe UI", 28, FontStyle.Bold, GraphicsUnit.Point);
            SizeF textSize = graphics.MeasureString(text, font);
            using var textBrush = new SolidBrush(Color.White);
            graphics.DrawString(text, font, textBrush,
                (size - textSize.Width) / 2f,
                (size - textSize.Height) / 2f);
        }

        return bitmap;
    }

    private static string GetDisplayText(string className)
    {
        className = className.Trim();
        if (className.Length <= 3)
        {
            return className.ToUpperInvariant();
        }

        return className[..3].ToUpperInvariant();
    }

    private Color GetColorForClass(string className)
    {
        if (palette.TryGetValue(className, out Color color))
        {
            return color;
        }

        int hash = Math.Abs(className.GetHashCode());
        int r = 80 + (hash % 150);
        int g = 80 + (hash / 10 % 150);
        int b = 80 + (hash / 100 % 150);
        return Color.FromArgb(r, g, b);
    }

    public void Dispose()
    {
        foreach (Image image in cache.Values)
        {
            image.Dispose();
        }

        cache.Clear();
    }
}
