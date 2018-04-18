using Ionic.Zip;
using System.IO;
using System.Reflection;

namespace ToolBox
{
    public class ZipHelper
    {
        public static MemoryStream ToMemoryStrem()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LuisActions.Samples.Assets.airportCatalog.zip";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var data = new MemoryStream();
                using (var zip = ZipFile.Read(stream))
                {
                    zip["airportCatalog.xml"].Extract(data);
                }

                data.Seek(0, SeekOrigin.Begin);
                return data;
            }
        }
    }
}
