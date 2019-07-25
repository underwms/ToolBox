using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ToolBox
{
    public class UTF8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public class XmlHelper
    {
        public static string ToXML<T>(T obj)
        {
            var xmlStr = string.Empty;
            var settings = new XmlWriterSettings()
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineChars = string.Empty,
                NewLineHandling = NewLineHandling.None
            };

            using (var stringWriter = new UTF8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);

                    var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    serializer.Serialize(xmlWriter, obj, namespaces);

                    xmlStr = stringWriter.ToString();
                }
            }

            return xmlStr;
        }


        public static string ListToXML<T>(IEnumerable<T> collection, string rootName = "ROOT")
        {
            var xmlStr = string.Empty;
            var settings = new XmlWriterSettings()
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineChars = string.Empty,
                NewLineHandling = NewLineHandling.None
            };

            using (var stringWriter = new UTF8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);

                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootName));
                    serializer.Serialize(xmlWriter, collection, namespaces);

                    xmlStr = stringWriter.ToString();
                }
            }

            return xmlStr;
        }

        private T ToObject<T>(string xml)
        {
            var obj = new object();
            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = serializer.Deserialize(xmlReader);
                }
            }

            return (T)obj;
        }
    }
}
