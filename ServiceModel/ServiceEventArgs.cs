using System;

namespace ServiceModel
{
    public class ServiceEventArgs
    {
        public static readonly ServiceEventArgs Empty = new ServiceEventArgs();

        public ServiceEventArgs()
        {
        }

        public ServiceEventArgs(string message) : this(ServiceEventType.Information, message, null)
        {
        }

        public ServiceEventArgs(ServiceEventType type, string message) : this(type, message, null)
        {
        }

        public ServiceEventArgs(ServiceEventType type, string message, Exception ex)
        {
            Type = type;
            Message = message;
            Exception = ex;
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp
        {
            get;
            private set;
        }

        public ServiceEventType Type
        {
            get;
            internal set;
        }

        public string Message
        {
            get;
            internal set;
        }

        public Exception Exception
        {
            get;
            internal set;
        }
    }
}
