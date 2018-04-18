namespace GenericTests
{
    using System.Xml.Serialization;

    public class Identity
    {
        private const string _businessId = "";
        private const string _login = "";
        private const string _notProd = "";
        private const string _prod = "";
        private static string _environment = string.Empty;

        public Identity()
            : this(Constants._environmentNotProdKey)
        { }

        public Identity(string environment)
        {
            _environment = environment;
            Password = (_environment == Constants._environmentProdKey) ? _prod : _notProd;
        }

        [XmlElement(ElementName = "business-id")]
        public string BusinessId { get; set; } = _businessId;

        [XmlElement(ElementName = "login")]
        public string Login { get; set; } = _login;

        [XmlElement(ElementName = "password")]
        public string Password { get; set; }
    }
}
