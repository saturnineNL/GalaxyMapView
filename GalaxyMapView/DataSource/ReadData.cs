using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMapView.DataSource
{
    static class ReadData
    {

        public static string ReadSystem()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            const string path = "GalaxyMapView.DataSource.systems.json";

            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string ReadStation()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            const string path = "GalaxyMapView.DataSource.stations.json";

            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string ReadCommodities()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            const string path = "GalaxyMapView.DataSource.commodities.json";

            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

        }
    }
}
