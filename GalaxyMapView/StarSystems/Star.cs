using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using GalaxyMapView._3D;

namespace GalaxyMapView.StarSystems
{
    public class Star
    {
        private SolidColorBrush foregroundBrush = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        private SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromArgb(0x40, 0x3D, 0x95, 0xDE));
        private SolidColorBrush fontBrush = new SolidColorBrush(Color.FromArgb(0xFF,0x7D,0xB5,0xDE));
        
        public Star()
        {
            centerX = 285;
            centerY = 285;

            starGFX = new Ellipse();
            starSEL = new Ellipse();

            starCanvas = new Canvas();
            starLabel = new Label();

            starSEL.Visibility = Visibility.Hidden;
            starGFX.Fill = backgroundBrush;

            starLabel.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Euro Caps");
            starLabel.Foreground = fontBrush;

            starCanvas.Children.Add(starGFX);
            starCanvas.Children.Add(starLabel);
            starCanvas.Children.Add(starSEL);

            movePoint = new Point3D(0,0,0);
            rotaPoint = new Point3D(0,0,0);

            minSize = -8;
            maxSize = 8;

            starCanvas.MouseDown += SetSelection;
        }

        private void SetSelection(object sender, RoutedEventArgs e)
        {
           
            selectSystem = !selectSystem;

            if (selectSystem)
            {
                starSEL.Visibility = Visibility.Visible;
            }

            if (!selectSystem)
            {
                starSEL.Visibility = Visibility.Hidden;
            }
        }

        public void InitStar()
        {
            Draw3D D3D = new Draw3D();

            Point3D projectPoint = new Point3D();

            if (zoom < 1)
            {
                zoom = 1;
            }

            scale = (256 / zoom);

            scalePoint = new Point3D(scale, scale, scale);

            projectPoint = D3D.DrawAzimuth(currentPoint, elevation, azimuth, rotaPoint, movePoint, scalePoint);

            renderDepth = (projectPoint.Z / centerX);

        }

        public void BuildName()
        {
            originalName = name;

            string[] splitter = name.Split(' ');

            string buildName = String.Empty;

            int length = 0;

            displayWidth = 0;
            displayHeight = 1;

            for(int wordCount=0;wordCount<splitter.Length;wordCount+=1)
            {
                if (length < 14)
                {
                    buildName += splitter[wordCount]+" ";

                    length += splitter[wordCount].Length+1;

                    if (displayWidth < length) displayWidth = length;
                }

                else
                {
                    buildName += "\n";

                    displayHeight += 1;

                    if (wordCount > 0)
                    {
                        wordCount -= 1;}

                    length = 0;
                }

            }

            displayName= buildName;
  
        }

        public void ShowNames()
        {
            starLabel.Visibility = Visibility.Hidden;

            if (size >= 14 && renderDepth > 0 && !centerSystem)
            {
                starLabel.Visibility = Visibility.Visible;

                starLabel.FontSize = size*0.7;

                starLabel.Width = size*displayWidth;
                starLabel.Height = (size*1.5)*displayHeight;

                starLabel.Foreground = fontBrush;

                starLabel.HorizontalContentAlignment = HorizontalAlignment.Left;
                starLabel.VerticalContentAlignment = VerticalAlignment.Center;

                starLabel.Margin = new Thickness(-(starGFX.Width/2) + size,
                    -(starGFX.Height/2) - ((size/3)*displayHeight), 0, 0);

                starLabel.Content = displayName;
            }

            if (centerSystem)
            {
                starLabel.Visibility = Visibility.Visible;

                starLabel.FontSize = 24 * 0.7;

                starLabel.Width = 24 * displayWidth;
                starLabel.Height = (24 * 1.5) * displayHeight;

                starLabel.HorizontalContentAlignment = HorizontalAlignment.Left;
                starLabel.VerticalContentAlignment = VerticalAlignment.Center;

                starLabel.Margin = new Thickness(-(starGFX.Width / 2) + 24,-(starGFX.Height / 2) - ((24 / 3) * displayHeight), 0, 0);

                starLabel.Content = displayName;

            }

        }

        public void SetSize()
        {
           
            double zoomFactor =(1.3 - renderDepth)/2;

            size = 2 + ((scale/zoomFactor)*0.35);

            starGFX.Width = size;
            starGFX.Height = size;
            starGFX.StrokeThickness = size/6;
            starGFX.Stroke= new SolidColorBrush(Color.FromArgb(0x40,0x00,0x00,0x00));

            if (centerSystem)
            {
                starGFX.Width = 18;
                starGFX.Height = 18;

                starSEL.Width = starGFX.Width * 1.5;
                starSEL.Height = starGFX.Height * 1.5;

                starSEL.StrokeThickness = 3;

                starSEL.Visibility = Visibility.Visible;

            }

            if (selectSystem)
            {
                starSEL.Width = starGFX.Width * 1.5;
                starSEL.Height = starGFX.Height * 1.5;

                starSEL.StrokeThickness = 2;
                starSEL.Visibility = Visibility.Visible;

            }
        }


        public void SetColor()
        {
            if (renderDepth >= 0 && (!centerSystem || !selectSystem))
            {
                starGFX.Fill = foregroundBrush;
                starLabel.Foreground = fontBrush;
                
            }

            if (renderDepth < 0 && (!centerSystem || !selectSystem))
            {

                starGFX.Fill = backgroundBrush;
                starLabel.Foreground = fontBrush;
            }

            if (centerSystem)
            {
                starGFX.Fill = new SolidColorBrush(Colors.AliceBlue); 
                starGFX.Opacity = 1;
                starSEL.Stroke = new SolidColorBrush(Colors.AliceBlue); 
                starLabel.Foreground = new SolidColorBrush(Colors.AliceBlue);
    
            }

            if (selectSystem)
            {
                starGFX.Fill = new SolidColorBrush(Colors.Crimson); 
                starGFX.Opacity = 1;
                starSEL.Stroke = new SolidColorBrush(Colors.Crimson); 
                starLabel.Foreground = new SolidColorBrush(Colors.Crimson);

                starLabel.Visibility = Visibility.Visible;
            }

        }

        public Canvas Project3D()
        {
            Draw3D D3D = new Draw3D();

            projectPoint = new Point3D();

            if (zoom < 1)
            {
                zoom = 1;
            }

            scale = (256 / zoom);

            scalePoint = new Point3D(scale, scale, scale);

            projectPoint = D3D.DrawAzimuth(currentPoint, elevation, azimuth, rotaPoint, movePoint, scalePoint);

            renderDepth = (projectPoint.Z / centerX);

            starGFX.Margin = new Thickness(-(starGFX.Width / 2), -(starGFX.Height / 2), 0, 0);
            
            starSEL.Margin = new Thickness(-(starGFX.Width * 1.5) / 2, -(starGFX.Height * 1.5) / 2, 0, 0);

            starCanvas.Margin = new Thickness(centerX +projectPoint.X, centerY + projectPoint.Y, 0, 0);

            return starCanvas;
        }

        public int starID { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string originalName { get; set; }
        public int displayWidth { get; set; }
        public int displayHeight { get; set; }
        public Ellipse starGFX { get; set; }
        public Ellipse starSEL { get; set; }

        public Label starLabel { get; set; }
        public Canvas starCanvas { get; set; }
        
        public Point3D currentPoint { get; set; }
        public Point3D originPoint { get; set; }
        public Point3D projectPoint { get; set; }
        public Point3D rotaPoint { get; set; }
        public Point3D movePoint { get; set; }
        public Point3D scalePoint { get; set; }

        public bool centerSystem { get; set; }
        public bool selectSystem { get; set; }

        public double distance { get; set; }
        public double renderDepth { get; set; }
        
        public double centerX { get; set; }
        public double centerY { get; set; }

        public double azimuth { get; set; }
        public double elevation { get; set; }

        public double minSize { get; set; }
        public double maxSize { get; set; }

        public double size { get; set; }
        public double zoom { get; set; }
        public double scale { get; set; }
    }
}
