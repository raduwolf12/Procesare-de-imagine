using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;
namespace ISIP_Algorithms.Tools
{
    public class FilterTools
    {
        public static Image<Gray, byte> FiltruGausian(Image<Gray, byte> InputImage, double sigma)

        {
            Image<Gray, byte> Result = AplicareMascaGausiana(InputImage, sigma);
            return Result;
        }

        public static Image<Gray, byte> AplicareMascaGausiana(Image<Gray, byte> srcImage, double sigma)
        {
            Image<Gray, byte> Result = srcImage.Clone();

            double[,] mask = GaussianMask(sigma);

            int width = srcImage.Width;
            int height = srcImage.Height;
            
            int interval = (mask.GetLength(0) - 1) / 2;

            
            for (int y = interval; y < height - interval; y++) 
            {
                for (int x = interval; x < width - interval; x++)
                {
                    double var = 0.0;
                    for (int i = -interval; i < interval; i++)
                    {
                        for (int j = -interval; j < interval; j++)
                        {
                            var += (srcImage.Data[y + i, x + j, 0] * mask[i + interval, j + interval]);
                        }
                    }


                    //if (var > 255)
                    //    var = 255;
                    //if (var < 0)
                    //    var = 0;
                    Result.Data[y, x, 0] = (byte)(var + 0.5);
                }
            }
            return Result;
        }
       
        public static double[,] GaussianMask(double sigma)
        {

            int dimensiuneMasca = (int)(4 * sigma)+1;

            if (dimensiuneMasca % 2 == 0)
            {
                dimensiuneMasca++;
            }
            
            double[,] masca = new double[dimensiuneMasca, dimensiuneMasca];

            double sumValMasca = 0;
            
            int interval = (int)(sigma * 2);

            for (int y = -interval; y <= interval; y++)
            {
                for (int x = -interval; x <= interval; x++)
                {
                    masca[y + interval, x + interval] = 1d / (2 * Math.PI * sigma * sigma) * Math.Exp(-((y * y) + (x * x)) / (2 * sigma * sigma));
                    sumValMasca += masca[y + interval, x + interval];
                }
            }

            for (int y = 0; y < dimensiuneMasca; y++)
            {
                for (int x = 0; x < dimensiuneMasca; x++)
                {
                    masca[y, x] = masca[y, x] * 1d / sumValMasca;
                }
            }
            return masca;
        }
       
        public static Image<Gray, byte> Filtrare_bilaterala(Image<Gray, byte> srcImage, double sigma, double sigma2)
        {
            Image<Gray, byte> Result = srcImage.Clone();

            double[,] mask = GaussianMask(sigma);

            int width = srcImage.Width;
            int height = srcImage.Height;

            int interval = (mask.GetLength(0) - 1) / 2;

            int dimMaska = mask.GetLength(0);


            for (int y = interval; y < height - interval; y++)
            {
                for (int x = interval; x < width - interval; x++)
                {
                    double var = 0.0;
                    double var2 = 0.0;
                    double var3 = 0.0;
                    for (int i = -interval; i < interval; i++)
                    {
                        for (int j = -interval; j < interval; j++)
                        {
                            var += (srcImage.Data[y + i, x + j, 0] * mask[i + interval, j + interval]) * H_r(srcImage.Data[y + i, x + j, 0] - srcImage.Data[y, x, 0], sigma2);
                            var2 += (mask[i + interval, j + interval]) * H_r(srcImage.Data[y + i, x + j, 0] - srcImage.Data[y, x, 0], sigma2);

                            //var += srcImage.Data[y + i, x + j, 0] * H_d(i + interval, j + interval, sigma) * H_r(srcImage.Data[y + i, x + j, 0] - srcImage.Data[y, x, 0], sigma2);
                            //var2 += H_d(i + interval, j + interval, sigma) * H_r(srcImage.Data[y + i, x + j, 0] - srcImage.Data[y, x, 0], sigma2);

                        }
                    }


                    var3 = var / var2;
                    //if (var3 > 255)
                    //    var3 = 255;
                    //if (var3 < 0)
                    //    var3 = 0;

                    Result.Data[y, x, 0] = (byte)(var3 +0.5);
                }
            }
            return Result;
        }

        public static double H_r(double x,double sigma)
        {
            double up = Math.Pow(x, 2);
            double down = 2 * Math.Pow(sigma, 2);
            double fraction = up / down;
            double val = Math.Exp(-fraction);
            return val;
        }
        public static double H_d(double i, double j, double sigma)
        {
            double up = Math.Pow(i, 2)+ Math.Pow(j, 2);
            double down = 2 * Math.Pow(sigma, 2);
            double fraction = up / down;
            double val = Math.Exp(-fraction);
            return val;
        }

        public static Image<Gray, byte> FiltruSobelOrizontal(Image<Gray, byte> InputImage, double treshold)
        {
            Image<Gray, byte> Result = Sobel(InputImage,treshold);
            return Result;
        }
       
        public static Image<Gray, byte> Sobel(Image<Gray, byte> InputImage, double t)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);            

            double[,] Sx = new double[3, 3] {{-1, 0, 1 }, {-2, 0, 2 }, {-1, 0, 1 }};
            double[,] Sy = new double[3, 3] {{-1, -2, -1 }, {0, 0, 0 }, {1, 2, 1 }};
            const double rad = 180 / Math.PI;

            for (int y = 1; y < InputImage.Height - 1; y++)
            {
                for (int x = 1; x < InputImage.Width - 1; x++)
                {
                    double Fx = 0;
                    double Fy = 0;
                    for (int i = 0; i <= 2; i++)
                    {
                        for (int j = 0; j <= 2; j++)
                        {
                            Fx += InputImage.Data[y + i - 1, x + j - 1, 0] * Sx[i, j];
                            Fy += InputImage.Data[y + i - 1, x + j - 1, 0] * Sy[i, j];
                        }
                    }

                    double grad = Math.Sqrt(Fx * Fx + Fy * Fy);
                    double theta = 0;

                    if (grad < t)
                    {
                        Result.Data[y, x, 0] = (byte)(0);
                    }
                    else
                    {
                        theta = Math.Atan2(Fy, Fx);
                        double grade = theta * rad;

                        if (grade >= -95 && grade <= -85)
                        {
                            Result.Data[y, x, 0] = (byte)(255);
                        }
                        else if (grade >= 85 && grade <= 95)
                        {
                            Result.Data[y, x, 0] = (byte)(255);
                        }

                        else
                        {
                            Result.Data[y, x, 0] = (byte)(0);
                        }
                    }
                }
            }

            return Result;
        }

    }
}
