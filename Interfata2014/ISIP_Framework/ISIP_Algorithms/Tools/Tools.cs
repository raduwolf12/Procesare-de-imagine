using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;
namespace ISIP_Algorithms.Tools
{
    public class Tools
    {
        public static Image<Gray, byte> Invert(Image<Gray, byte> InputImage)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    Result.Data[y, x, 0] = (byte)(255 - InputImage.Data[y, x, 0]);
                }
            }
            return Result;
        }
        public static List<int> EMLookup(double m, double E)
        {
            double c = Math.Pow(m, E) / (Math.Pow(255, E + 1) + Math.Pow(m, E) * 255);

            List<int> LUT = new List<int>();
            for (int i = 0; i < 256; i++)
            {
                double f = 255 * (Math.Pow(i, E) / (Math.Pow(i, E) + Math.Pow(m, E)) + c * i);
                LUT.Add((int)f);
            }
            return LUT;
        }
        public static Image<Gray, byte> EM(Image<Gray, byte> InputImage, double m, double E)
        {
            List<int> EMLut = EMLookup(m, E);

            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    Result.Data[y, x, 0] = (byte)(EMLut.ElementAt(InputImage.Data[y, x, 0]));
                }
            }

            return Result;
        }

        public static Image<Gray, byte> Binarizare_color_3D(Image<Bgr, byte> InputImage, int xT, int yT, double treshhold)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);

            Bgr pixel = InputImage[yT, xT];

            double b0 = pixel.Blue;
            double g0 = pixel.Green;
            double r0 = pixel.Red;

            Gray white = new Gray(255);
            Gray black = new Gray(0);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    Bgr pixelx = InputImage[y, x];

                    double bx = pixelx.Blue;
                    double gx = pixelx.Green;
                    double rx = pixelx.Red;
                    double d = Math.Sqrt(Math.Pow(rx - r0, 2) + Math.Pow(gx - g0, 2) + Math.Pow(bx - b0, 2));
                    if (d <= treshhold)
                    {
                        Result[y, x] = white;
                    }
                    else
                    {
                        Result[y, x] = black;
                    }

                }
            }
            return Result;
        }

        public static Image<Gray, byte> Binarizare_color_2D(Image<Bgr, byte> InputImage, int xT, int yT, double treshhold)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);

            Bgr pixel = InputImage[yT, xT];

            double g0 = pixel.Green;
            double r0 = pixel.Red;
            double b0 = pixel.Blue;


            double nr0 = r0 / (r0 + g0 + b0);
            double ng0 = g0 / (r0 + g0 + b0);

            Gray white = new Gray(255);
            Gray black = new Gray(0);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    Bgr pixelx = InputImage[y, x];
                    double r = pixelx.Red;
                    double g = pixelx.Green;
                    double b = pixelx.Blue;

                    double nr = r / (r + g + b);
                    double ng = g / (r + g + b);

                    double d = Math.Sqrt(Math.Pow(nr - nr0, 2) + Math.Pow(ng - ng0, 2));
                    if (d <= treshhold)
                    {
                        Result[y, x] = white;
                    }
                    else
                    {
                        Result[y, x] = black;
                    }
                }
            }

            return Result;
        }

     
    }
}
