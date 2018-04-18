namespace GenericTests
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "add-billing-account-response", Namespace = _xmlNameSpace)]
    public class AddBillingAccountResponse : BaseResponse, IACIResponseEntity
    {
        private const string _xmlNameSpace = "";

        [XmlIgnore]
        public string XmlNameSpace => _xmlNameSpace;

        [XmlElement(ElementName = "fubar")]
        public string Fubar { get; set; }
    }
}
