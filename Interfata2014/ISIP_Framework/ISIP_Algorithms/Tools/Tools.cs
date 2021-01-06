using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

        public static Image<Gray, byte> Binarizare(double prag, Image<Gray, byte> originalImage)
        {
            Image<Gray, byte> resultImage = new Image<Gray, byte>(originalImage.Size);

            for (int index1 = 0; index1 < originalImage.Height; index1++)
                for (int index2 = 0; index2 < originalImage.Width; index2++)
                {
                    if (originalImage.Data[index1, index2, 0] < prag)
                    {
                        resultImage.Data[index1, index2, 0] = 0;
                    }
                    else
                    {
                        resultImage.Data[index1, index2, 0] = 255;
                    }
                }
            return resultImage;
        }

        public static Image<Gray,byte> Dilatare(Image<Gray, byte> imgBinarizata)
        {
            Image<Gray, byte> resultImage = new Image<Gray, byte>(imgBinarizata.Data);

            for (int y = 1; y < imgBinarizata.Height - 1; y++)
            {
                for (int x = 1; x < imgBinarizata.Width - 1; x++)
                {
                    if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y, x - 1, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y - 1, x - 1, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y - 1, x, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y + 1, x + 1, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y , x + 1, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y-1, x + 1, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y + 1, x , 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 255 && imgBinarizata.Data[y + 1, x - 1, 0] == 0)
                    {
                        resultImage.Data[y, x, 0] = 255;
                    }
                }
            }
            return resultImage;
        }
        public static Image<Gray, byte> Erodare(Image<Gray, byte> imgBinarizata)
        {
            Image<Gray, byte> resultImage = new Image<Gray, byte>(imgBinarizata.Data);

            for (int y = 1; y < imgBinarizata.Height - 1; y++)
            {
                for (int x = 1; x < imgBinarizata.Width - 1; x++)
                {
                    if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y, x - 1, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y - 1, x - 1, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y - 1, x, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y + 1, x + 1, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y, x + 1, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y - 1, x + 1, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y + 1, x, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                    else
                         if (imgBinarizata.Data[y, x, 0] == 0 && imgBinarizata.Data[y + 1, x - 1, 0] == 255)
                    {
                        resultImage.Data[y, x, 0] = 0;
                    }
                }
            }
            return resultImage;
        }
        public static Image<Gray, byte> XorDilatare(double thresold, Image<Gray, byte> originalImage)
        {
            Image<Gray, byte> resultImage = new Image<Gray, byte>(originalImage.Size);
            resultImage = Binarizare(thresold, originalImage);

            Image<Gray, byte> resultImageDilatare = Dilatare(resultImage);

            Image<Gray, byte> resultImageXor = new Image<Gray, byte>(resultImage.Size);


            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    if (resultImage.Data[y, x, 0] == 0 && resultImageDilatare.Data[y, x, 0] == 0)
                    {
                        resultImageXor.Data[y, x, 0] = 0;
                    }
                    else if (resultImage.Data[y, x, 0] == 0 && resultImageDilatare.Data[y, x, 0] == 255)
                    {
                        resultImageXor.Data[y, x, 0] = 255;
                    }
                    else if (resultImage.Data[y, x, 0] == 255 && resultImageDilatare.Data[y, x, 0] == 0)
                    {
                        resultImageXor.Data[y, x, 0] = 255;
                    }
                    else
                    {
                        resultImageXor.Data[y, x, 0] = 0;
                    }
                }
            }

            return resultImageXor;

        }
        public static Image<Gray, byte> XorErodare(double thresold, Image<Gray, byte> originalImage)
        {
            Image<Gray, byte> resultImage = new Image<Gray, byte>(originalImage.Size);
            resultImage = Binarizare(thresold, originalImage);

            Image<Gray, byte> resultImageErodare = Erodare(resultImage);

            Image<Gray, byte> resultImageXor = new Image<Gray, byte>(resultImage.Size);


            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    if (resultImage.Data[y, x, 0] == 0 && resultImageErodare.Data[y, x, 0] == 0)
                    {
                        resultImageXor.Data[y, x, 0] = 0;
                    }
                    else if (resultImage.Data[y, x, 0] == 0 && resultImageErodare.Data[y, x, 0] == 255)
                    {
                        resultImageXor.Data[y, x, 0] = 255;
                    }
                    else if (resultImage.Data[y, x, 0] == 255 && resultImageErodare.Data[y, x, 0] == 0)
                    {
                        resultImageXor.Data[y, x, 0] = 255;
                    }
                    else
                    {
                        resultImageXor.Data[y, x, 0] = 0;
                    }
                }
            }

            return resultImageXor;

        }

        public static Image<Gray, byte> Rotatia(double unghi, Image<Gray, byte> originalImage)
        {
            Image<Gray, byte> resultImage = new Image<Gray, byte>(originalImage.Size);

            double grade = unghi * (Math.PI / 180);

            double xO = resultImage.Height / 2;
            double yO = resultImage.Width / 2;



            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {

                    double xc = Math.Cos(grade) * (x - xO) + Math.Sin(grade) * (y - yO) + xO;
                    double yc = -Math.Sin(grade) * (x - xO) + Math.Cos(grade) * (y - yO) + yO;


                    int x0 = (int)xc;
                    int y0 = (int)yc;

                    if (x0 > 0 && y0 > 0 && x0 < 255 && y0 < 255)
                    {
                        double val1 = (originalImage.Data[y0, x0 + 1, 0] - originalImage.Data[y0, x0, 0]) * (xc - x0) + originalImage.Data[y0, x0, 0];
                        double val2 = (originalImage.Data[y0 + 1, x0 + 1, 0] - originalImage.Data[y0 + 1, x0, 0]) * (xc - x0) + originalImage.Data[y0 + 1, x0, 0];
                        byte val3 = (byte)((val2 - val1) * (yc - y0) + val1);

                        resultImage.Data[y, x, 0] = val3;
                    }
                    else
                        resultImage.Data[y, x, 0] = 0;

                }
            }

            return resultImage;
        }


       
        public static Image<Gray,byte> Hough_rapida( Image<Gray, byte> InputImage, double thresold=100)
        {
            // doua cadrane si razele negative
            double d = Math.Sqrt(InputImage.Height * InputImage.Height + InputImage.Width * InputImage.Width);

            Image<Gray, byte> resultImage = new Image<Gray, byte>( 2* (int)(d + 0.5) + 1, 181);


            double[,] Sx = new double[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            double[,] Sy = new double[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            int[,] matrix = new int[2 * (int)(d + 0.5) + 1, 181];

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
                            double val = Math.Sqrt(Fx * Fx + Fy * Fy);
                            if (val >= thresold)
                            {
                                //calculez unghiul teta care este atan din fy,fx
                                var theta = Math.PI / 2;
                                if (Fx!=0)
                                {
                                     theta = Math.Atan(Fy / Fx);
                                }
                                
                                var unghi = theta * 180 / Math.PI;
                           
                                var r = x * Math.Cos(theta) + y * Math.Sin(theta);
                            
                                matrix[(int)(r + 0.5) + (int)d, (int)unghi + 90]++;
                            }                    
                }
            }

            for (int y = 0; y <= 2 * (int)(d + 0.5); y++)
            {
                for (int x = 0; x < 181; x++)
                {
                    resultImage.Data[x, y, 0] = (byte)( matrix[y, x] + 0.5);
                }
            }


            Image<Gray, byte> resultImage2 = new Image<Gray, byte>(2 * (int)(d + 0.5) + 1, 181);
            for (int y = 0; y <= 2 * (int)(d + 0.5); y++)
            {
                for (int x = 0; x < 181; x++)
                {
                    resultImage2.Data[180-x, y, 0] = resultImage.Data[x, y, 0];
                }
            }

            return resultImage2; 
        }
   
        public static Image<Gray, byte> Desenare_Hough_rapid(Image<Gray, byte> InputImage, Image<Gray, byte> houghImage)
        {
            double d = Math.Sqrt(InputImage.Height * InputImage.Height + InputImage.Width * InputImage.Width);

            Image<Gray, byte> resultImage = new Image<Gray, byte>(2 * (int)(d + 0.5) + 1, 181);
            List<Point> coordonate = new List<Point>();

            for (int y = 0; y <= 2 * (int)(d + 0.5); y++)
            {
                for (int x = 0; x < 181; x++)
                {

                    if(houghImage.Data[x,y,0]>50)
                    {

                        Point p = new Point(y, x);
                        coordonate.Add(p);

                    }


                }
            }
            Point pMaxLocal = new Point(0, 0);
            Point pMaxLocal1 = new Point(0, 0);
            List<Point> coordonate2 = new List<Point>();

            for (int i =0;i<coordonate.Count;i++)
            {
                pMaxLocal = ComparePoint(pMaxLocal, coordonate.ElementAt(i));
                if(pMaxLocal!=pMaxLocal1)
                {
                    coordonate2.Add(pMaxLocal);
                }
                pMaxLocal1 = pMaxLocal;
            }
            List<Point> coordonate3 = new List<Point>();
            for (int i = 0; i < coordonate2.Count; i++)
            {
                double r = coordonate2.ElementAt(i).X - d;
                double teta = (coordonate2.ElementAt(i).Y - 90)/(180 / Math.PI);
                int x = (int)(r * Math.Cos(teta));
                int y = (int)(r * Math.Sin(teta));
                coordonate3.Add(new Point(x, y));
            }
            //int r1,r2;
            //int t1, t2;
            //var dreapta = Math.Sqrt(r1 * r1 + r2 * r2 - 2 * r2 * r2 * Math.Cos(t2 - t1); 
            //x = r cos θ, y = r sin θ


            //for (int y = 0; y < InputImage.Height; y++)
            //     {
            //        for (int x = 0; x < InputImage.Width; x++)
            //        {
                   
            //        }
            //    }


                    return null;
        }
        public static Point ComparePoint(Point p1, Point p2)
        {
            if(p1.X ==0 && p1.Y == 0)
            {
                return p2;
            }
            if (p1.X - 60 <= p2.X && p2.X <= p1.X + 60)
            {
                return p1;
            } else
                return p2;
            
        }

    }

}
