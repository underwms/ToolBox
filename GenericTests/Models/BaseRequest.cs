using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTests
{
    using System.Xml.Serialization;
    
    public abstract class BaseRequest
    {
        private string _environment = string.Empty;
        private string _urlEndPointProd = string.Empty;
        private string _urlEndPointNotProd = string.Empty;
        private Identity _identity;
        
        public BaseRequest(
            string environment,
            string urlEndPointProd,
            string urlEndPointNotProd)
        {
            _environment = environment;
            _urlEndPointProd = urlEndPointProd;
            _urlEndPointNotProd = urlEndPointNotProd;
            _identity = new Identity(environment);
        }
        
        public string URLEndPoint => (_environment == Constants._environmentProdKey) ? _urlEndPointProd : _urlEndPointNotProd;

        public Identity ACIIdentity => _identity;
    }
}
