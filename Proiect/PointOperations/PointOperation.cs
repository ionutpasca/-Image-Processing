using System;
using System.Drawing;

namespace PointOperations
{
    public static class PointOperation
    {
        public static Bitmap ImageToDigitalNegative(this Bitmap image)
        {
            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var pixelColor = image.GetPixel(i, j);
                    var a = pixelColor.A;
                    var r = pixelColor.R;
                    var g = pixelColor.G;
                    var b = pixelColor.B;

                    r = (byte)(255 - r);
                    g = (byte)(255 - g);
                    b = (byte)(255 - b);

                    image.SetPixel(i, j, Color.FromArgb(a, r, g, b));
                }
            }
            return image;
        }

        public static Bitmap ImageWithContrastStretch(this Bitmap image)
        {
            var min = 255;
            var max = 0;
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var rgb = image.GetPixel(x, y);
                    var r = rgb.R;
                    var g = rgb.G;
                    var b = rgb.B;
                    if (min > r) min = r;
                    if (min > g) min = g;
                    if (min > b) min = b;
                    if (max < r) max = r;
                    if (max < g) max = g;
                    if (max < b) max = b;
                }
            }
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var rgb = image.GetPixel(x, y);
                    var r = (rgb.R - min) * 255 / (max - min);
                    var g = (rgb.G - min) * 255 / (max - min);
                    var b = (rgb.B - min) * 255 / (max - min);
                    image.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return image;
        }

        public static Bitmap ImageTreshold(this Bitmap image, byte threshholdLevel)
        {
            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var color = image.GetPixel(i, j);
                    var pixel = (color.R + color.G + color.B) / 3;
                    image.SetPixel(i, j,
                        pixel < threshholdLevel ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255));
                }
            }
            return image;
        }

        public static Bitmap ImageColorFilter(this Bitmap image, ColorFilterType colorFilter)
        {
            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    var r = 0;
                    var g = 0;
                    var b = 0;
                    if (colorFilter == ColorFilterType.Red)
                    {
                        r = pixel.R;
                        g = pixel.G - 255;
                        b = pixel.B - 255;
                    }
                    if (colorFilter == ColorFilterType.Green)
                    {
                        r = pixel.R - 255;
                        g = pixel.G;
                        b = pixel.B - 255;
                    }
                    if (colorFilter == ColorFilterType.Blue)
                    {
                        r = pixel.R - 255;
                        g = pixel.G - 255;
                        b = pixel.B;
                    }
                    r = Math.Max(r, 0);
                    r = Math.Min(255, r);

                    g = Math.Max(g, 0);
                    g = Math.Min(255, g);

                    b = Math.Max(b, 0);
                    b = Math.Min(255, b);

                    image.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return image;
        }

        public static Bitmap ImageGammaFilter(this Bitmap image, double red, double green, double blue)
        {
            var redGamma = CreateGammaArray(red);
            var greenGamma = CreateGammaArray(green);
            var blueGamma = CreateGammaArray(blue);
            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var color = image.GetPixel(i, j);
                    image.SetPixel(i, j,
                        Color.FromArgb(redGamma[color.R], greenGamma[color.G], blueGamma[color.B]));
                }
            }
            return image;
        }

        private static byte[] CreateGammaArray(double color)
        {
            var gammaArray = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                gammaArray[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
            }
            return gammaArray;
        }

        public static Bitmap ImageBrightness(this Bitmap image, int brightness)
        {
            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;

            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var color = image.GetPixel(i, j);
                    var R = color.R + brightness;
                    var G = color.G + brightness;
                    var B = color.B + brightness;
                    if (R < 0) R = 1;
                    if (R > 255) R = 255;

                    if (G < 0) G = 1;
                    if (G > 255) G = 255;

                    if (B < 0) B = 1;
                    if (B > 255) B = 255;

                    image.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
            return image;
        }

        public static Bitmap ImageGrayscale(this Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    var gray = (byte)(.299 * pixel.R + .587 * pixel.G + .114 * pixel.B);
                    image.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return image;
        }
    }

    public enum ColorFilterType
    {
        Red,
        Green,
        Blue
    }
}
