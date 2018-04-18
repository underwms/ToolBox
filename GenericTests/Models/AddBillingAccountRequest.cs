namespace GenericTests
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "add-billing-account-request", Namespace = _xmlNameSpace)]
    public class AddBillingAccountRequest : BaseRequest, IACIRequestEntity
    {
        private const string _xmlNameSpace = "";
        private const string _urlEndPointProd = "";
        private const string _urlEndPointNotProd = "";

        public AddBillingAccountRequest()
            : base(
                Constants._environmentNotProdKey,
                _urlEndPointProd,
                _urlEndPointNotProd)
        { }

        public AddBillingAccountRequest(string environemt)
            : base(
                environemt,
                _urlEndPointProd,
                _urlEndPointNotProd)
        { }

        [XmlIgnore()]
        public string XmlNameSpace => _xmlNameSpace;

        [XmlElement(ElementName = "identity")]
        public Identity Identity { get { return ACIIdentity; } set { } }

        [XmlElement(ElementName = "billing-account-number")]
        public int AccountId { get; set; }
    } 
}
