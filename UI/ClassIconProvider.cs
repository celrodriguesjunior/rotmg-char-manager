using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
        const int size = 200;
        const string dir = "C:\\dev\\meuu\\rotmg-char-manager\\img";

        Bitmap bitmap;

        bitmap = new Bitmap($"{dir}\\{className.ToLower()}.png");
        //bitmap = GerarImagemQuadrada($"{dir}\\{className.ToLower()}.png");

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
    //public static Bitmap GerarImagemQuadrada(string caminhoEntrada)
    //{
    //    using var original = Image.FromFile(caminhoEntrada);

    //    int largura = original.Width;
    //    int altura = original.Height;

    //    int lado = Math.Max(largura, altura); // tamanho do quadrado

    //    // Garante transparência (PNG)
    //    using var quadrada = new Bitmap(lado, lado, PixelFormat.Format32bppArgb);

    //    using (var g = Graphics.FromImage(quadrada))
    //    {
    //        // Fundo transparente
    //        g.Clear(Color.Transparent);

    //        // Pra pixel art não ficar borrado
    //        g.InterpolationMode = InterpolationMode.NearestNeighbor;
    //        g.PixelOffsetMode = PixelOffsetMode.Half;
    //        g.SmoothingMode = SmoothingMode.None;
    //        g.CompositingMode = CompositingMode.SourceOver;

    //        // Centraliza a imagem
    //        int x = (lado - largura) / 2;
    //        int y = (lado - altura) / 2;

    //        g.DrawImage(original, x, y, largura, altura);
    //    }

    //    return quadrada;
    //}

}
