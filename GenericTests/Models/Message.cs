namespace GenericTests
{
    using System.Xml.Serialization;

    public class Message
    {
        [XmlElement(ElementName = "message-code")]
        public int MessageCode { get; set; }

        [XmlElement(ElementName = "message-text")]
        public string MessageText { get; set; }
    }
}
