using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace GalaxyMapView._3D
{
    public class Draw3D
    {
        public Draw3D()
        {
            
        }

        public Point3D DrawAzimuth(Point3D inputPoint,double elevation, double azimuth,
            Point3D rotation=default(Point3D),Point3D translation=default(Point3D),Point3D scaling=default(Point3D))
        {
            Point3D projectedPoint = new Point3D();

            bool doRotationX = false;
            bool doRotationY = false;
            bool doRotationZ = false;

            bool doTranslation = false;
            bool doScaling = false;

            if (!Point3D.Equals(new Point3D(0, 0, 0), rotation))
            {
                if (rotation.X != 0.0) doRotationX = true;
                if (rotation.Y != 0.0) doRotationY = true;
                if (rotation.Z != 0.0) doRotationZ = true;
            }

            if (!Point3D.Equals(new Point3D(0, 0, 0), translation)) doTranslation = true;
            if (!Point3D.Equals(new Point3D(0, 0, 0), scaling)) doScaling = true;

          //  if (doTranslation)
                inputPoint = CalculateTranslation(inputPoint, translation);

            if (doRotationX) inputPoint = CalculateRotationX(inputPoint, rotation.X);
            if (doRotationY) inputPoint = CalculateRotationY(inputPoint, rotation.Y);
            if (doRotationZ) inputPoint = CalculateRotationZ(inputPoint, rotation.Z);

           
            if (doScaling) inputPoint = CalculateScaling(inputPoint, scaling);

            projectedPoint = CalculateAzimuth(inputPoint, elevation, azimuth);

            return projectedPoint;
        }

        public Point3D CalculateAzimuth(Point3D input,double elevation,double azimuth)
        {
       
            Point3[] pts = new Point3[1];

            pts[0] = new Point3(input.X, input.Y, input.Z,0);

            Matrix3 m = Matrix3.AzimuthElevation(elevation,azimuth,0);

            pts[0].Transform(m);

            Point3D output = new Point3D(pts[0].X,pts[0].Y,pts[0].Z);

            return output;

        }

        public Point3D CalculateRotationX(Point3D input, double rotaX)
        {
            Point3[] pts = new Point3[1];

            pts[0] = new Point3(input.X,input.Y,input.Z,0);

            Matrix3 m = Matrix3.Rotate3X(rotaX);

            pts[0].Transform(m);

            Point3D output = new Point3D(pts[0].X, pts[0].Y, pts[0].Z);

            return output;
        }

        public Point3D CalculateRotationY(Point3D input, double rotaY)
        {
            Point3[] pts = new Point3[1];

            pts[0] = new Point3(input.X, input.Y, input.Z, 0);

            Matrix3 m = Matrix3.Rotate3Y(rotaY);

            pts[0].Transform(m);

            Point3D output = new Point3D(pts[0].X, pts[0].Y, pts[0].Z);

            return output;
        }

        public Point3D CalculateRotationZ(Point3D input, double rotaZ)
        {
            Point3[] pts = new Point3[1];

            pts[0] = new Point3(input.X, input.Y, input.Z, 0);

            Matrix3 m = Matrix3.Rotate3Z(rotaZ);

            pts[0].Transform(m);

            Point3D output = new Point3D(pts[0].X, pts[0].Y, pts[0].Z);

            return output;
        }

        public Point3D CalculateTranslation(Point3D input,Point3D displacement)
        {
            Point3[] pts = new Point3[1];

            pts[0] = new Point3(input.X, input.Y, input.Z, 0);

            Matrix3 m = Matrix3.Translate3(displacement.X,displacement.Y,displacement.Z);

            pts[0].Transform(m);

            Point3D output = new Point3D(pts[0].X, pts[0].Y, pts[0].Z);

            return output;
        }

        public Point3D CalculateScaling(Point3D input, Point3D scaling)
        {
            Point3[] pts = new Point3[1];

            pts[0] = new Point3(input.X, input.Y, input.Z, 0);

            Matrix3 m = Matrix3.Scale3(scaling.X, scaling.Y, scaling.Z);

            pts[0].Transform(m);

            Point3D output = new Point3D(pts[0].X, pts[0].Y, pts[0].Z);

            return output;
        }

        public void DrawFront()
        {
            
        }

        public void DrawSide()
        {
            
        }

        public void DrawTop()
        {
            
        }
    }
}
