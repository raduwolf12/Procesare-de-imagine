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
        //public static List<int> EM(double r, double m, double E)
        public static List<int> EMLookup(double m= 127.5, double E= 2.5)
        {
            double c = Math.Pow(m, E) / (Math.Pow(255, E+1)+ Math.Pow(m, E)*255);

            List<int> LUT = new List<int>();            
            for (int i = 0; i<255; i++)
            {
                double f =255 * (Math.Pow(i, E) / (Math.Pow(i, E) + Math.Pow(m, E)) + c*i);
                LUT.Add((int)f);
            }
            return LUT;
        }
        public static Image<Gray, byte> EM(Image<Gray, byte> InputImage)
        {
            List<int> EMLut = EMLookup();

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
    }
}
