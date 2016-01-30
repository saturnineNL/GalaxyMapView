using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Newtonsoft.Json;
using GalaxyMapView.DataSource;
using GalaxyMapView.UserControls;

namespace GalaxyMapView.StarSystems
{

    public class StarSystems
    {

        public StarSystems()
        {
            starSystemSet = BuildCollection();

            string currentSystemName = "Beta Volantis";

            starSystemList = UpdateStarSystemList(currentSystemName,-64,64);

            starList = BuildStarList(starSystemList);

        }
        
        public Dictionary<int,StarSystem> BuildCollection()
        {
            String systemDataSet = ReadData.ReadSystem();

            Dictionary<int, StarSystem> starSystemCollection = new Dictionary<int, StarSystem>();

            dynamic readSystem = JsonConvert.DeserializeObject(systemDataSet);

            int collectionCounter = 0;

            foreach (var systemData in readSystem)
            {
                starSystemCollection[collectionCounter] = new StarSystem();

                    starSystemCollection[collectionCounter].id = systemData.id;
                    starSystemCollection[collectionCounter].name = systemData.name;
                    starSystemCollection[collectionCounter].X = systemData.x;
                    starSystemCollection[collectionCounter].Y = systemData.y;
                    starSystemCollection[collectionCounter].Z = systemData.z;
                    starSystemCollection[collectionCounter].faction = systemData.faction;
                    starSystemCollection[collectionCounter].population = systemData.population;
                    starSystemCollection[collectionCounter].government = systemData.government;
                    starSystemCollection[collectionCounter].allegiance = systemData.allegiance;
                    starSystemCollection[collectionCounter].state = systemData.state;
                    starSystemCollection[collectionCounter].security = systemData.security;
                    starSystemCollection[collectionCounter].primary_economy = systemData.primary_economy;
                    starSystemCollection[collectionCounter].power = systemData.power;
                    starSystemCollection[collectionCounter].power_state = systemData.power_state;
                    starSystemCollection[collectionCounter].simbad_ref = systemData.simbad_ref;

                collectionCounter += 1;

            }

            return starSystemCollection;
        }      

        public List<StarSystem> BuildStarSystemList(Point3D midPoint, double range)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            double minX = midPoint.X - range; double minY = midPoint.Y - range; double minZ = midPoint.Z - range;
            double maxX = midPoint.X + range; double maxY = midPoint.Y + range; double maxZ = midPoint.Z + range;

            foreach (var systemlist in starSystemSet
                .Where(system => system.Value.X >= minX && system.Value.X <= maxX &&
                                 system.Value.Y >= minY && system.Value.Y <= maxY &&
                                 system.Value.Z >= minZ && system.Value.Z <= maxZ))
            {
                if (calculateDistance(midPoint,new Point3D(systemlist.Value.X, systemlist.Value.Y, systemlist.Value.Z)) <= range)
                {
                    tempList.Add(systemlist.Value);
                }
            }

            centerPoint = midPoint;

            return tempList; 

        }

        public List<StarSystem> BuildStarSystemList(Point3D midPoint, double minRange,double maxRange)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            double minX = midPoint.X + minRange; double minY = midPoint.Y + minRange; double minZ = midPoint.Z + minRange;
            double maxX = midPoint.X + maxRange; double maxY = midPoint.Y + maxRange; double maxZ = midPoint.Z + maxRange;

            double range=Math.Max(Math.Abs(minRange),Math.Abs(maxRange));

            foreach (var systemlist in starSystemSet
                .Where(system => system.Value.X >= minX && system.Value.X <= maxX &&
                                 system.Value.Y >= minY && system.Value.Y <= maxY &&
                                 system.Value.Z >= minZ && system.Value.Z <= maxZ))
            {
                if (calculateDistance(midPoint, new Point3D(systemlist.Value.X, systemlist.Value.Y, systemlist.Value.Z)) <= range)
                {
                    tempList.Add(systemlist.Value);
                }
            }

            centerPoint = midPoint;

            return tempList;

        }

        

        public StarSystem GetCurrentSystem(int systemID)
        {
            StarSystem currentStarSystem = new StarSystem();

            foreach (var result in starSystemSet.Where(sysID => sysID.Value.id == systemID))
            {

                currentStarSystem = result.Value;
                currentSystemID = result.Value.id;

                break;
            }

            return currentStarSystem;

        }

        public StarSystem GetCurrentSystem(string systemName)
        {
            StarSystem currentStarSystem = new StarSystem();

            foreach (var result in starSystemSet.Where(sysName => sysName.Value.name == systemName))
            {

                currentStarSystem = result.Value;
                currentSystemID = result.Value.id;

                break;
            }

            return currentStarSystem;
        }

        public List<StarSystem> UpdateStarSystemList(string systemName, double range)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            StarSystem centerSystem = GetCurrentSystem(systemName);

            centerPoint = new Point3D(centerSystem.X, centerSystem.Y, centerSystem.Z);

            tempList = BuildStarSystemList(centerPoint, range);

            return tempList;
        }

        public List<StarSystem> UpdateStarSystemList(string systemName, double minRange, double maxRange)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            StarSystem centerSystem = GetCurrentSystem(systemName);

            centerPoint = new Point3D(centerSystem.X, centerSystem.Y, centerSystem.Z);

            tempList = BuildStarSystemList(centerPoint, minRange, maxRange);

            return tempList;
        }

        public List<StarSystem> UpdateStarSystemList(int systemID, double range)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            StarSystem centerSystem = GetCurrentSystem(systemID);

            centerPoint = new Point3D(centerSystem.X, centerSystem.Y, centerSystem.Z);

            tempList = BuildStarSystemList(centerPoint, range);

            return tempList;
        }

        public List<StarSystem> UpdateStarSystemList(int systemID, double minRange, double maxRange)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            StarSystem centerSystem = GetCurrentSystem(systemID);

            centerPoint = new Point3D(centerSystem.X, centerSystem.Y, centerSystem.Z);

            tempList = BuildStarSystemList(centerPoint, minRange,maxRange);

            return tempList;
        }

        public List<StarSystem> UpdateStarSystemList(Point3D centerPoint, double range)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            tempList = BuildStarSystemList(centerPoint, range);

            return tempList;
        }

        public List<StarSystem> UpdateStarSystemList(Point3D centerPoint, double minRange, double maxRange)
        {
            List<StarSystem> tempList = new List<StarSystem>();

            tempList = BuildStarSystemList(centerPoint, minRange, maxRange);

            return tempList;
        }

        private double calculateDistance(Point3D startPoint, Point3D endPoint)
        {

            double distanceX = 99999999;
            double distanceY = 99999999;
            double distanceZ = 99999999;

            if (startPoint.X <= 0 && endPoint.X < 0) distanceX = Math.Min(startPoint.X, endPoint.X) + Math.Abs(Math.Max(startPoint.X, endPoint.X));
            if (startPoint.Y <= 0 && endPoint.Y < 0) distanceY = Math.Min(startPoint.Y, endPoint.Y) + Math.Abs(Math.Max(startPoint.Y, endPoint.Y));
            if (startPoint.Z <= 0 && endPoint.Z < 0) distanceZ = Math.Min(startPoint.Z, endPoint.Z) + Math.Abs(Math.Max(startPoint.Z, endPoint.Z));

            if ((startPoint.X <= 0 && endPoint.X > 0) || (startPoint.X > 0 && endPoint.X < 0)) distanceX = Math.Abs(Math.Min(startPoint.X, endPoint.X)) + Math.Max(startPoint.X, endPoint.X);
            if ((startPoint.Y <= 0 && endPoint.Y > 0) || (startPoint.Y > 0 && endPoint.Y < 0)) distanceY = Math.Abs(Math.Min(startPoint.Y, endPoint.Y)) + Math.Max(startPoint.Y, endPoint.Y);
            if ((startPoint.Z <= 0 && endPoint.Z > 0) || (startPoint.Z > 0 && endPoint.Z < 0)) distanceZ = Math.Abs(Math.Min(startPoint.Z, endPoint.Z)) + Math.Max(startPoint.Z, endPoint.Z);

            if (startPoint.X >= 0 && endPoint.X > 0) distanceX = Math.Max(startPoint.X, endPoint.X) - Math.Min(startPoint.X, endPoint.X);
            if (startPoint.Y >= 0 && endPoint.Y > 0) distanceY = Math.Max(startPoint.Y, endPoint.Y) - Math.Min(startPoint.Y, endPoint.Y);
            if (startPoint.Z >= 0 && endPoint.Z > 0) distanceZ = Math.Max(startPoint.Z, endPoint.Z) - Math.Min(startPoint.Z, endPoint.Z);

            if (startPoint.X == endPoint.X) distanceX = 0;
            if (startPoint.Y == endPoint.Y) distanceY = 0;

            double distance = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2) + Math.Pow(distanceZ, 2));

            return distance;

        }

        public List<Star> BuildStarList(List<StarSystem> collection)
        {
            List<Star> tempList = new List<Star>();

            foreach (var starSystem in collection.OrderBy(order => order.Z))
            {
                Star star = new Star();

                star.starID = starSystem.id;
                star.name = starSystem.name;

                star.centerX = 285;
                star.centerY = 285;

                star.zoom = 64;

                star.originPoint = new Point3D(starSystem.X, starSystem.Y, starSystem.Z);

                star.currentPoint = new Point3D(centerPoint.X - starSystem.X, centerPoint.Y - starSystem.Y, centerPoint.Z - starSystem.Z);

                star.distance = calculateDistance(centerPoint, star.originPoint);

                star.BuildName();

                star.InitStar();

               // star.SetSize();

               // star.SetColor();

             //   star.ShowNames();

                tempList.Add(star);

            }

            return tempList;
        }


        public Dictionary<int, StarSystem> starSystemSet { get; set; }
        public List<StarSystem> starSystemList { get; set; }
        public List<Star> starList { get; set; }
        public Point3D centerPoint { get; set; }
        public int currentSystemID { get; set; }
        
    }

    public class StarSystem
    { 

        public int id { get; set; }

        public string name { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public string faction { get; set; }
        public string population { get; set; }

        public string government { get; set; }
        public string allegiance { get; set; }

        public string state { get; set; }
        public string security { get; set; }

        public string primary_economy { get; set; }

        public string power { get; set; }
        public string power_state { get; set; }

        public string simbad_ref { get; set; }

    }
}
