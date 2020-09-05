/*
 * Author : kharre
 * Date : 2020/09/05
 */

using System;
using System.Drawing;

namespace fptascii
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fileName = "";

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

            Bitmap picture = new Bitmap(fileName, true);

            // Numeric value array of shades of gray from the picture
            short[,] shade = new short[picture.Width, picture.Height];

            // Character array for ASCII representation of the picture
            char[,] ascii = new char[picture.Width, picture.Height];

            // Pixel per pixel processing
            for (short x = 0; x < picture.Width; x++)
            {
                for (short y = 0; y < picture.Height; y++)
                {
                    Color currentPixelColor = picture.GetPixel(x, y);
                    // Get shade of gray of the current pixel
                    shade[x, y] = (byte)((currentPixelColor.R + currentPixelColor.G + currentPixelColor.B) / 3);
                    // Set a character for the current shade of gray
                    if (shade[x, y] <= 255 && shade[x, y] > 192)
                    {
                        ascii[x, y] = '.';
                    }
                    else if (shade[x, y] <= 192 && shade[x, y] > 128)
                    {
                        ascii[x, y] = 'o';
                    }
                    else if (shade[x, y] <= 128 && shade[x, y] > 64)
                    {
                        ascii[x, y] = '0';
                    }
                    else if (shade[x, y] <= 64 && shade[x, y] >= 0)
                    {
                        ascii[x, y] = '@';
                    }
                }
            }

            string[] lines = new string[picture.Height];

            for (short x = 0; x < picture.Height; x++)
            {
                string line = "";
                for (short y = 0; y < picture.Width; y++)
                {
                    line += ascii[y, x] + "  ";
                }
                lines[x] = line;
            }

            System.IO.File.WriteAllLines($"{fileName}.txt", lines);
        }
    }
}
