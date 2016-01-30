using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaxyMapView.StarSystems;
using GalaxyMapView._3D;

namespace GalaxyMapView.UserControls
{
    /// <summary>
    /// Interaction logic for GalaxyMap.xaml
    /// </summary>
    public partial class GalaxyMap : UserControl
    {

        private Draw3D draw3D = new Draw3D();

        private StarSystems.StarSystems starSystemCollection = new StarSystems.StarSystems();

        

        private DispatcherTimer rotator = new DispatcherTimer();

        private Point3D rotatorPoint = new Point3D();
        private Point3D moverPoint = new Point3D(0, 0, 0);

        private StarSystem currentSystem;

        private double zoom = 32;
        private double elevation = 0;
        private double azimuth = 0;

        private bool leftMouseButton = false;
        private bool rightMouseButton = false;

        private Point mouseReferencePoint = new Point();

        private Line lineX = new Line();
        private Line lineY = new Line();
        private Line lineZ = new Line();

        public Canvas GalaxyMapCanvas = new Canvas();

        public Label headerLabel = new Label();
        public Canvas StarDisplay = new Canvas();
        public Canvas OverlayDisplay = new Canvas();

        private Dictionary<int, StarSystem> starSystemSet;
        private List<StarSystem> starSystemList;
        private List<Star> starList;

        public GalaxyMap()
        {
            InitializeComponent();

            GalaxyMapCanvas.Width = 570;
            GalaxyMapCanvas.Height = 570;
            GalaxyMapCanvas.HorizontalAlignment= HorizontalAlignment.Left;
            GalaxyMapCanvas.VerticalAlignment = VerticalAlignment.Top;

            GalaxyMapCanvas.Background=new SolidColorBrush(Colors.Aquamarine);

            StarDisplay.Width = GalaxyMapCanvas.Width;
            StarDisplay.Height = GalaxyMapCanvas.Height;
            StarDisplay.HorizontalAlignment = HorizontalAlignment.Left;
            StarDisplay.VerticalAlignment = VerticalAlignment.Top;

            StarDisplay.Background = new SolidColorBrush(Color.FromArgb(0xFF,0x09,0x08,0x29));

            GalaxyMapCanvas.Children.Add(StarDisplay);

            OverlayDisplay.Width = 5;
            OverlayDisplay.Height = 5;
            OverlayDisplay.HorizontalAlignment=HorizontalAlignment.Center;
            OverlayDisplay.VerticalAlignment=VerticalAlignment.Center;

            OverlayDisplay.Background = new SolidColorBrush(Color.FromArgb(0x00,0xF9,0x08,0x29));

            headerLabel.Width = 550;
            headerLabel.Height = 30;
            headerLabel.HorizontalAlignment=HorizontalAlignment.Left;
            headerLabel.VerticalAlignment=VerticalAlignment.Top;
            headerLabel.Margin=new Thickness(10);

            headerLabel.Foreground = new SolidColorBrush(Colors.White);

            GalaxyMapCanvas.Children.Add(headerLabel);

            GalaxyMapGrid.Children.Add(GalaxyMapCanvas);
            GalaxyMapGrid.Children.Add(OverlayDisplay);

            GalaxyMapCanvas.MouseMove += MouseMove_Handler;
            GalaxyMapCanvas.MouseLeftButtonDown += LeftMouseButtonDown_Handler;
            GalaxyMapCanvas.MouseLeftButtonUp += LeftMouseButtonUp_Handler;
            GalaxyMapCanvas.MouseRightButtonDown += RightMouseButtonDown_Handler;
            GalaxyMapCanvas.MouseRightButtonUp += RightMouseButtonUp_Handler;
            GalaxyMapCanvas.MouseWheel += MouseWheel_Handler;

            starSystemSet = starSystemCollection.starSystemSet;

            starSystemList = starSystemCollection.starSystemList;

            starList = starSystemCollection.starList;

            BuildEdge();

            BuildOverlay();

            currentSystem = starSystemCollection.GetCurrentSystem("Beta Volantis");

            starSystemList = starSystemCollection.UpdateStarSystemList("Beta Volantis", -zoom, zoom);

            starList = starSystemCollection.BuildStarList(starSystemList);

            RenderStars();

            rotator.Tick += Rotator_Tick;
            rotator.Interval = TimeSpan.FromMilliseconds(50);
            rotator.Start();
        }

        public void BuildEdge()
        {
            Ellipse edge = new Ellipse();

            edge.Height = edge.Width = 538;

            edge.HorizontalAlignment = HorizontalAlignment.Center;
            edge.VerticalAlignment = VerticalAlignment.Center;

            edge.Margin = new Thickness(16,16,285,285);

            edge.Stroke = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
            edge.StrokeThickness = 3;

            GalaxyMapCanvas.Children.Add(edge);
        }


        public void BuildOverlay()
        {
            UpdateOverlay();

            lineX.Stroke = new SolidColorBrush(Colors.Blue);
            lineX.StrokeThickness = 3;

            OverlayDisplay.Children.Add(lineX);

            lineY.Stroke = new SolidColorBrush(Colors.DarkGreen);
            lineY.StrokeThickness = 3;

            OverlayDisplay.Children.Add(lineY);

            lineZ.Stroke = new SolidColorBrush(Colors.Crimson);
            lineZ.StrokeThickness = 3;

            OverlayDisplay.Children.Add(lineZ);

            

        }

        public void UpdateOverlay()
        {
            Point3D scale = new Point3D(4,4,4);
            Point3D move = new Point3D(0, 0, 0);
            Point3D rota = new Point3D(0, 0, rotatorPoint.Z);

            double centerX = OverlayDisplay.Width/2;
            double centerY = OverlayDisplay.Height/2;

            Point3D center = draw3D.DrawAzimuth(new Point3D(0, 0, 0), elevation, azimuth, rota, move, scale);
            Point3D xAxis = draw3D.DrawAzimuth(new Point3D(10, 0, 0), elevation, azimuth, rota, move, scale);
            Point3D yAxis = draw3D.DrawAzimuth(new Point3D(0, 10, 0), elevation, azimuth, rota, move, scale);
            Point3D zAxis = draw3D.DrawAzimuth(new Point3D(0, 0, -10), elevation, azimuth, rota, move, scale);

            lineX.X1 = centerX + center.X;
            lineY.X1 = centerX + center.X;
            lineZ.X1 = centerX + center.X;

            lineX.Y1 = centerY + center.Y;
            lineY.Y1 = centerY + center.Y;
            lineZ.Y1 = centerY + center.Y;

            lineX.X2 = centerX + xAxis.X;
            lineY.X2 = centerX + yAxis.X;
            lineZ.X2 = centerX + zAxis.X;

            lineX.Y2 = centerY + xAxis.Y;
            lineY.Y2 = centerY + yAxis.Y;
            lineZ.Y2 = centerY + zAxis.Y;

        }

        public void RenderStars()
        {
            StarDisplay.Children.Clear();

            foreach (var star in starList.OrderBy(order => order.projectPoint.Z))
            {

                StarDisplay.Children.Add(star.starCanvas);

                star.ShowNames();

                star.centerSystem = star.starID == currentSystem.id;

                star.elevation = elevation;

                star.azimuth = azimuth;

                star.zoom = zoom;

                star.rotaPoint = rotatorPoint;

                star.SetColor();

                star.SetSize();

                star.Project3D();
                

                headerLabel.Content = currentSystem.name + " E: " + elevation.ToString("F") + " A: " + azimuth.ToString("F") + " Z: " + zoom + " SC: " + starList.Count + " scale: " + star.scale.ToString("F");
            }
        }

        public void UpdateRender()
        {

            foreach (var star in starList)
            {
                star.centerSystem = star.starID == currentSystem.id;

                star.rotaPoint = rotatorPoint;

                UpdateOverlay();

                star.elevation = elevation;
                star.azimuth = azimuth;
                star.zoom = zoom;

                star.SetSize();

                star.ShowNames();

                star.Project3D();

                star.SetColor();

                headerLabel.Content = currentSystem.name + " E: " + elevation.ToString("F") + " A: " + azimuth.ToString("F") + " Z: " + zoom + " SC: " + starList.Count + " scale: " + star.scale.ToString("F");
            }
        }

        private void MoveToSystem()
        {
            
        }

        private void Rotator_Tick(object sender, EventArgs e)
        {

            UpdateRender();

            rotatorPoint.Z += 0.5;

        }

        private void LeftMouseButtonDown_Handler(object sender, RoutedEventArgs e)
        {
           

            if (!leftMouseButton )
            {
                mouseReferencePoint.X = Mouse.GetPosition(GalaxyMapCanvas).X - (GalaxyMapCanvas.Width/2);
                mouseReferencePoint.Y = Mouse.GetPosition(GalaxyMapCanvas).Y - (GalaxyMapCanvas.Height/2);

                leftMouseButton = true;
            }
        }

        private void LeftMouseButtonUp_Handler(object sender, MouseButtonEventArgs e)
        {
            leftMouseButton = false;
        }

        private void RightMouseButtonDown_Handler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (!rightMouseButton)
            {
                mouseReferencePoint.X = Mouse.GetPosition(GalaxyMapCanvas).X - (GalaxyMapCanvas.Width / 2);
                mouseReferencePoint.Y = Mouse.GetPosition(GalaxyMapCanvas).Y - (GalaxyMapCanvas.Height / 2);

                rightMouseButton = true;

            }
        }

        private void RightMouseButtonUp_Handler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            rightMouseButton = false;
        }

        private void MouseMove_Handler(object sender, MouseEventArgs e)
        {
            Point movePoint = new Point();

            movePoint.X = Mouse.GetPosition(GalaxyMapCanvas).X - (GalaxyMapCanvas.Width/2);
            movePoint.Y = Mouse.GetPosition(GalaxyMapCanvas).Y - (GalaxyMapCanvas.Width/2);

            double el = 0;
            double az = 0;

            if (leftMouseButton && !rightMouseButton)
            {
                az = ((mouseReferencePoint.X - movePoint.X)/ GalaxyMapCanvas.Width)*128;
                el = ((mouseReferencePoint.Y - movePoint.Y)/ GalaxyMapCanvas.Height)*128;

                elevation += el;
                azimuth += az;
            }

            double moveX = 0;
            double moveY = 0;
            double moveZ = 0;

            if (rightMouseButton)
            {
                if (!leftMouseButton) moveZ = ((mouseReferencePoint.Y - movePoint.Y)/ GalaxyMapCanvas.Height)*128;

                else
                {
                    moveX = ((mouseReferencePoint.X - movePoint.X) / GalaxyMapCanvas.Width) * 128;
                    moveY = ((mouseReferencePoint.Y - movePoint.Y) / GalaxyMapCanvas.Height) * 128;
                } 
                Point3D movement = new Point3D(currentSystem.X-moveX,currentSystem.Y-moveY,currentSystem.Z-moveZ);

                starSystemList = starSystemCollection.UpdateStarSystemList(movement, -zoom, zoom);

                starList = starSystemCollection.BuildStarList(starSystemList);

                currentSystem.X -= moveX;
                currentSystem.Y -= moveY;
                currentSystem.Z -= moveZ;

                RenderStars();
            }

            mouseReferencePoint.X = movePoint.X;
            mouseReferencePoint.Y = movePoint.Y;
        }

        private void MouseWheel_Handler(object sender, MouseWheelEventArgs e)
        {

            double delta = (e.Delta/(120*0.5));

            zoom -= delta;

            if (zoom > 80)
            {
                zoom = 80;}

            if (zoom < 4)
            {
                zoom = 4;}
            starSystemList = starSystemCollection.UpdateStarSystemList(currentSystem.id, -zoom, zoom);

            starList = starSystemCollection.BuildStarList(starSystemList);

            starList.OrderBy(order => order.projectPoint.Z);
 
            RenderStars();

            UpdateRender();
        }

    }

}
