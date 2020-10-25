/*
 * Author : kharre
 * Creation : 2020/09/05
 * Modifications :
 * _2020/10/25 : Notion of scales added
 */

using System;
using System.Drawing;

namespace fptascii
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileName = "";

            // Needs scales in order to make the ASCII output visible without unzoom
            byte scaleX = 8;
            byte scaleY = scaleX;

            if (fileName.Equals(""))
            {
                // Add support of scales argument somehere
                switch (args[0])
                {
                    case "--help":
                    case "-h":
                        Console.WriteLine("Here is the help.");
                        Environment.Exit(0);
                        break;
                    default:
                        fileName = args[0].ToString();
                        break;
                }
            }

            // Source image
            Bitmap source = new Bitmap(fileName, true);

            // Numeric value array of shades of gray from the picture
            byte[,] shade = new byte[source.Width, source.Height];

            // Character array for ASCII representation of the picture
            char[,] ascii = new char[source.Width, source.Height];

            // Source image with a lower resolution
            Bitmap lowRes = new Bitmap(source.Width / scaleX, source.Height / scaleY);

            for (var x = 0; x < source.Width - scaleX; x += scaleX)
            {
                for (var y = 0; y < source.Height - scaleY; y += scaleY)
                {
                    Color currentPixelColor = source.GetPixel(x, y);
                    lowRes.SetPixel(x / scaleX, y / scaleY, currentPixelColor);
                }
            }

            // Pixel per pixel processing
            for (var x = 0; x < lowRes.Width; x++)
            {
                for (var y = 0; y < lowRes.Height; y++)
                {
                    Color currentPixelColor = lowRes.GetPixel(x, y);
                    // Get shade of gray of the current pixel
                    shade[x, y] = (byte)((currentPixelColor.R + currentPixelColor.G + currentPixelColor.B) / 3);
                    // Set a character for the current shade of gray
                    if ((shade[x, y] <= 255) && (shade[x, y] > 192))
                    {
                        ascii[x, y] = '.';
                    }
                    else if ((shade[x, y] <= 192) && (shade[x, y] > 128))
                    {
                        ascii[x, y] = 'o';
                    }
                    else if ((shade[x, y] <= 128) && (shade[x, y] > 64))
                    {
                        ascii[x, y] = '0';
                    }
                    else if ((shade[x, y] <= 64) && (shade[x, y] >= 0))
                    {
                        ascii[x, y] = '@';
                    }
                }
            }

            string[] lines = new string[source.Height];

            for (var x = 0; x < lowRes.Height; x++)
            {
                var line = "";
                for (var y = 0; y < lowRes.Width; y++)
                {
                    line += ascii[y, x] + "  ";
                }
                lines[x] = line;
            }

            System.IO.File.WriteAllLines($"{fileName}.txt", lines);
        }
    }
}
