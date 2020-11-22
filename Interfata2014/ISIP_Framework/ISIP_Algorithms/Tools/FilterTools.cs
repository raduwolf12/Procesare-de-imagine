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
            
            int foff = (mask.GetLength(0) - 1) / 2;

            
            for (int y = foff; y < height - foff; y++) 
            {
                for (int x = foff; x < width - foff; x++)
                {
                    double var = 0.0;
                    for (int i = -foff; i < foff; i++) 
                    {
                        for (int j = -foff; j < foff; j++) 
                        {
                            var += (srcImage.Data[y + i, x + j, 0] * mask[i + foff, j + foff]);
                            
                        }
                    }
                    if (var > 255)
                        var = 255;
                    if (var < 0)
                        var = 0;
                    Result.Data[y, x, 0] = (byte)(var + 0.5);
                }
            }
            return Result;
        }

        public static double[,] GaussianMask(double sigma)
        {

            int dimensiuneMasca = (int)(4 * sigma)+1;
            double[,] masca = new double[dimensiuneMasca, dimensiuneMasca];

            double sumValMasca = 0;

            int interval = (int)(sigma * 2);

            double distance = 0;
            double constant = 1d / (2 * Math.PI * sigma * sigma);
            for (int y = -interval; y <= interval; y++)
            {
                for (int x = -interval; x <= interval; x++)
                {
                    distance = ((y * y) + (x * x)) / (2 * sigma * sigma);
                    masca[y + interval, x + interval] = constant * Math.Exp(-distance);
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

            int foff = (mask.GetLength(0) - 1) / 2;


            for (int y = foff; y < height - foff; y++)
            {
                for (int x = foff; x < width - foff; x++)
                {
                    double var = 0.0;
                    double var2 = 0.0;
                    double var3 = 0.0;
                    for (int i = -foff; i < foff; i++)
                    {
                        for (int j = -foff; j < foff; j++)
                        {
                            var += (srcImage.Data[y + i, x + j, 0] * mask[i + foff, j + foff]) * H_r(srcImage.Data[y + i , x + j , 0] - srcImage.Data[y, x, 0], sigma2);
                            var2 += (mask[i + foff, j + foff]) * H_r(srcImage.Data[y + i , x + j , 0] - srcImage.Data[y, x, 0], sigma2);
                            
                            //var += srcImage.Data[y + i, x + j, 0] * H_d(i + foff, j + foff, sigma) * H_r(srcImage.Data[y + i, x + j, 0] - srcImage.Data[y, x, 0], sigma2);
                            //var2 += H_d(i + foff, j + foff, sigma) * H_r(srcImage.Data[y + i, x + j, 0] - srcImage.Data[y, x, 0], sigma2);

                        }
                    }
                    var3 = var / var2;
                    if (var3 > 255)
                        var3 = 255;
                    if (var3 < 0)
                        var3 = 0;

                    Result.Data[y, x, 0] = (byte)(var3 );
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


    }
}
