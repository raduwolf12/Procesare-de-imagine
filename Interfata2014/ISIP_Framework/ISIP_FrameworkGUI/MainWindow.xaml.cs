using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using ISIP_UserControlLibrary;

using ISIP_Algorithms.Tools;
using ISIP_FrameworkHelpers;
using System.Threading;

namespace ISIP_FrameworkGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    
    public partial class MainWindow : Window
    {
        //private Windows.Grafica dialog;
        Windows.Magnifyer MagnifWindow;
        Windows.GLine RowDisplay;
        bool Magif_SHOW = false;
        bool GL_ROW_SHOW = false;
        bool BIN_COLOR_2D = false;
        bool BIN_COLOR_3D = false;
        System.Windows.Point lastClick = new System.Windows.Point(0, 0);
        System.Windows.Point upClick = new System.Windows.Point(0, 0);

        public MainWindow()
        {
            InitializeComponent();
            mainControl.OriginalImageCanvas.MouseDown += new MouseButtonEventHandler(OriginalImageCanvas_MouseDown);
         }

        void OriginalImageCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastClick = Mouse.GetPosition(mainControl.OriginalImageCanvas);
            DrawHelper.RemoveAllLines(mainControl.OriginalImageCanvas);
            DrawHelper.RemoveAllRectangles(mainControl.OriginalImageCanvas);
            DrawHelper.RemoveAllLines(mainControl.ProcessedImageCanvas);
            DrawHelper.RemoveAllRectangles(mainControl.ProcessedImageCanvas);
            if (GL_ROW_ON.IsChecked)
            {
                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(0, lastClick.Y),
                     new System.Windows.Point(mainControl.OriginalImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                if (mainControl.ProcessedGrayscaleImage != null)
                {
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(0, lastClick.Y),
                     new System.Windows.Point(mainControl.ProcessedImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                }
                if (mainControl.OriginalGrayscaleImage != null) RowDisplay.Redraw((int)lastClick.Y);

            }
            if (Magnifyer_ON.IsChecked)
            {
                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(0, lastClick.Y),
                    new System.Windows.Point(mainControl.OriginalImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(lastClick.X, 0),
                    new System.Windows.Point(lastClick.X, mainControl.OriginalImageCanvas.Height - 1), System.Windows.Media.Brushes.Red, 1);
                DrawHelper.DrawAndGetRectangle(mainControl.OriginalImageCanvas, new System.Windows.Point(lastClick.X - 4, lastClick.Y - 4),
                    new System.Windows.Point(lastClick.X + 4, lastClick.Y + 4), System.Windows.Media.Brushes.Red);
                if (mainControl.ProcessedGrayscaleImage != null)
                {
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(0, lastClick.Y),
                    new System.Windows.Point(mainControl.ProcessedImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(lastClick.X, 0),
                        new System.Windows.Point(lastClick.X, mainControl.ProcessedImageCanvas.Height - 1), System.Windows.Media.Brushes.Red, 1);
                    DrawHelper.DrawAndGetRectangle(mainControl.ProcessedImageCanvas, new System.Windows.Point(lastClick.X - 4, lastClick.Y - 4),
                        new System.Windows.Point(lastClick.X + 4, lastClick.Y + 4), System.Windows.Media.Brushes.Red);
                }
                if (mainControl.OriginalGrayscaleImage != null) MagnifWindow.RedrawMagnifyer(lastClick);
            }

            if (BIN_COLOR_2D_ON.IsChecked)
            {
                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(0, lastClick.Y),
                   new System.Windows.Point(mainControl.OriginalImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(lastClick.X, 0),
                    new System.Windows.Point(lastClick.X, mainControl.OriginalImageCanvas.Height - 1), System.Windows.Media.Brushes.Red, 1);
                DrawHelper.DrawAndGetRectangle(mainControl.OriginalImageCanvas, new System.Windows.Point(lastClick.X - 4, lastClick.Y - 4),
                    new System.Windows.Point(lastClick.X + 4, lastClick.Y + 4), System.Windows.Media.Brushes.Red);
                if (mainControl.ProcessedGrayscaleImage != null)
                {
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(0, lastClick.Y),
                    new System.Windows.Point(mainControl.ProcessedImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(lastClick.X, 0),
                        new System.Windows.Point(lastClick.X, mainControl.ProcessedImageCanvas.Height - 1), System.Windows.Media.Brushes.Red, 1);
                    DrawHelper.DrawAndGetRectangle(mainControl.ProcessedImageCanvas, new System.Windows.Point(lastClick.X - 4, lastClick.Y - 4),
                        new System.Windows.Point(lastClick.X + 4, lastClick.Y + 4), System.Windows.Media.Brushes.Red);
                }
                UserInputDialog dlg = new UserInputDialog("3D Dialog", new string[] { "threshold:" });

                if (dlg.ShowDialog().Value == true)
                {
                    double t1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = Tools.Binarizare_color_2D(mainControl.OriginalColorImage, (int)lastClick.X, (int)lastClick.Y, t1);
                }
            }
            if (BIN_COLOR_3D_ON.IsChecked)
            {

                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(0, lastClick.Y),
                    new System.Windows.Point(mainControl.OriginalImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                DrawHelper.DrawAndGetLine(mainControl.OriginalImageCanvas, new System.Windows.Point(lastClick.X, 0),
                    new System.Windows.Point(lastClick.X, mainControl.OriginalImageCanvas.Height - 1), System.Windows.Media.Brushes.Red, 1);
                DrawHelper.DrawAndGetRectangle(mainControl.OriginalImageCanvas, new System.Windows.Point(lastClick.X - 4, lastClick.Y - 4),
                    new System.Windows.Point(lastClick.X + 4, lastClick.Y + 4), System.Windows.Media.Brushes.Red);
                if (mainControl.ProcessedGrayscaleImage != null)
                {
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(0, lastClick.Y),
                    new System.Windows.Point(mainControl.ProcessedImageCanvas.Width - 1, lastClick.Y), System.Windows.Media.Brushes.Red, 1);
                    DrawHelper.DrawAndGetLine(mainControl.ProcessedImageCanvas, new System.Windows.Point(lastClick.X, 0),
                        new System.Windows.Point(lastClick.X, mainControl.ProcessedImageCanvas.Height - 1), System.Windows.Media.Brushes.Red, 1);
                    DrawHelper.DrawAndGetRectangle(mainControl.ProcessedImageCanvas, new System.Windows.Point(lastClick.X - 4, lastClick.Y - 4),
                        new System.Windows.Point(lastClick.X + 4, lastClick.Y + 4), System.Windows.Media.Brushes.Red);
                }
                UserInputDialog dlg = new UserInputDialog("3D Dialog", new string[] { "threshold:" });

                if (dlg.ShowDialog().Value == true)
                {
                    double t1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = Tools.Binarizare_color_3D(mainControl.OriginalColorImage, (int)lastClick.X, (int)lastClick.Y, t1);
                }
            }

        }

        private void openGrayscaleImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            mainControl.LoadImageDialog(ImageType.Grayscale);
            Magnifyer_ON.IsEnabled = true;
            GL_ROW_ON.IsEnabled = true;
           
        }

        private void openColorImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            mainControl.LoadImageDialog(ImageType.Color);
            Magnifyer_ON.IsEnabled = true;
            GL_ROW_ON.IsEnabled = true;
            BIN_COLOR_2D_ON.IsEnabled = true;
            BIN_COLOR_3D_ON.IsEnabled = true;
        }

        private void saveProcessedImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!mainControl.SaveProcessedImageToDisk())
            {
                MessageBox.Show("Processed image not available!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void saveAsOriginalMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (mainControl.ProcessedGrayscaleImage != null)
            {
                mainControl.OriginalGrayscaleImage = mainControl.ProcessedGrayscaleImage;
            }
            else if(mainControl.ProcessedColorImage != null)
            {
                mainControl.OriginalColorImage = mainControl.ProcessedColorImage;
            }
        }

        private void Invert_Click(object sender, RoutedEventArgs e)
        {
            if (mainControl.OriginalGrayscaleImage != null)
            {

                mainControl.ProcessedGrayscaleImage=Tools.Invert(mainControl.OriginalGrayscaleImage);
            }

        }

        private void Magnifyer_ON_Click(object sender, RoutedEventArgs e)
        {
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (Magif_SHOW == true)
                {
                    Magif_SHOW = false;
                    MagnifWindow.Close();
                    DrawHelper.RemoveAllLines(mainControl.OriginalImageCanvas);
                    DrawHelper.RemoveAllRectangles(mainControl.OriginalImageCanvas);
                    DrawHelper.RemoveAllLines(mainControl.ProcessedImageCanvas);
                    DrawHelper.RemoveAllRectangles(mainControl.ProcessedImageCanvas);

                }
                else Magif_SHOW = true;
                if (Magif_SHOW == true)
                {
                    MagnifWindow = new Windows.Magnifyer(mainControl.OriginalGrayscaleImage, mainControl.ProcessedGrayscaleImage);
                    MagnifWindow.Show();
                    MagnifWindow.RedrawMagnifyer(lastClick);
                }
            }

        }

        private void GL_ROW_ON_Click(object sender, RoutedEventArgs e)
        {
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (GL_ROW_SHOW == true)
                {
                    GL_ROW_SHOW = false;
                    RowDisplay.Close();
                    DrawHelper.RemoveAllLines(mainControl.OriginalImageCanvas);
                    DrawHelper.RemoveAllLines(mainControl.ProcessedImageCanvas);
                }
                else GL_ROW_SHOW = true;

                if (GL_ROW_SHOW == true)
                {
                    RowDisplay = new Windows.GLine(mainControl.OriginalGrayscaleImage, mainControl.ProcessedGrayscaleImage);

                    RowDisplay.Show();
                    RowDisplay.Redraw((int)lastClick.Y);

                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (mainControl.OriginalGrayscaleImage != null)
            {
                UserInputDialog dlg = new UserInputDialog("Binar", new string[] { "prag1", "prag2" });
                if(dlg.ShowDialog().Value==true)
                {
                    int t1 = (int)dlg.Values[0];
                    int t2 = (int)dlg.Values[1];
                    //draw helper
                }
            }
        }
        
        private void Em_Click(object sender, RoutedEventArgs e)
        {

            UserInputDialog dlg = new UserInputDialog("EM Dialog", new string[] { "m value:", "E value:" });
           
            if (mainControl.OriginalGrayscaleImage != null)
                {
                if (dlg.ShowDialog().Value == true)
                {
                    double t1 = (double)dlg.Values[0];
                    double t2 = (double)dlg.Values[1];
                    mainControl.ProcessedGrayscaleImage = Tools.EM(mainControl.OriginalGrayscaleImage, t1, t2);
                }
            }
        }


        private void Bnarizare_Color_3D(object sender, RoutedEventArgs e)
        {
            if (mainControl.OriginalColorImage != null)
            {
                if (BIN_COLOR_3D == true)
                {
                  
                }
                else BIN_COLOR_3D = true;

                
            }
        }

        private void Bnarizare_Color_2D(object sender, RoutedEventArgs e)
        {
            if (mainControl.OriginalColorImage != null)
            {
                if (BIN_COLOR_2D == true)
                {

                }
                else BIN_COLOR_2D = true;


            }
        }

        private void Binarizare(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Binarizare", new string[] { "σd value: " });

            if (mainControl.OriginalGrayscaleImage != null)
            {

                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = Tools.Binarizare(d1, mainControl.OriginalGrayscaleImage);
                }
            }
        }
        private void Filtrare_bilaterala_Click(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Filtrare bilaterala Dialog", new string[] { "σd value:", "σr value:" });
            //UserInputDialog dlg = new UserInputDialog("Filtrare Gausian Dialog", new string[] { "σd value: " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    double d2 = (double)dlg.Values[1];
                    mainControl.ProcessedGrayscaleImage = FilterTools.Filtrare_bilaterala(mainControl.OriginalGrayscaleImage, d1,d2);
                }
            }
        }
        private void Filtrare_gausiana_Click(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Filtrare Gausian Dialog", new string[] { "σd value: " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = FilterTools.FiltruGausian(mainControl.OriginalGrayscaleImage, d1);
                }
            }
        }

        private void Sobel_orizontal_Click(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Filtrul directional Sobel - margini orizontale Dialog", new string[] { "t value: " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = FilterTools.FiltruSobelOrizontal(mainControl.OriginalGrayscaleImage, d1);
                }
            }
        }
        private void Sobel_vertical_Click(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Filtrul directional Sobel - margini orizontale Dialog", new string[] { "t value: " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = FilterTools.FiltruSobelVertical(mainControl.OriginalGrayscaleImage, d1);
                }
            }
        }

        private void XOR_Click(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Valoare thresold binarizare ", new string[] { "t value: " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = Tools.XorDilatare(d1, mainControl.OriginalGrayscaleImage);
                }
            }
        }
        private void XOR_ClickErodare(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Valoare thresold binarizare ", new string[] { "t value: " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = Tools.XorErodare(d1, mainControl.OriginalGrayscaleImage);
                }
            }
        }

        private void Rotatie_Click(object sender, RoutedEventArgs e)
        {
            UserInputDialog dlg = new UserInputDialog("Unghiul de rotatie(grade): ", new string[] { "unghiul de rotatie(grade): " });
            if (mainControl.OriginalGrayscaleImage != null)
            {
                if (dlg.ShowDialog().Value == true)
                {
                    double d1 = (double)dlg.Values[0];
                    mainControl.ProcessedGrayscaleImage = Tools.Rotatia(d1, mainControl.OriginalGrayscaleImage);
                }
            }
        }
    }
}
