using System.Xml.Serialization;

namespace GenericTests
{
    public class BaseResponse
    {
        [XmlElement(ElementName = "message")]
        public Message Message { get; set; }
    }
}
