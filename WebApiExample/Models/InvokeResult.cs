using System.Runtime.Serialization;

namespace WebApiExample.Models
{
    [DataContract]
    public class InvokeResult : OperationOutput
    {
        [DataMember]
        public string Trace
        {
            get;
            set;
        }

        [DataMember]
        public object Result
        {
            get;
            set;
        }
    }
}
