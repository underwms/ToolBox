using System;
using System.Runtime.Serialization;
using WebApiExample.Interfaces;

namespace WebApiExample.Models
{
    [DataContract]
    public class OperationResult<T> : IOperationResult where T : class, new()
    {
        public OperationResult()
        {
            Flags = OperationResultFlags.Succeeded;
        }

        [DataMember]
        private T _data;
        public T Data
        {
            get
            {
                if (null == _data)
                {
                    _data = new T();
                }
                return _data;
            }
            internal set
            {
                _data = value;
            }
        }

        public bool HasData
        {
            get { return (null != Data); }
        }

        [DataMember]
        public OperationResultFlags Flags
        {
            get;
            internal set;
        }

        [DataMember]
        public string Message
        {
            get;
            internal set;
        }

        public bool HasMessage
        {
            get { return (!String.IsNullOrEmpty(Message)); }
        }

        public bool Succeeded
        {
            get
            {
                return (Flags.HasFlag(OperationResultFlags.Succeeded));
            }
        }

        void IOperationResult.ClearFlags()
        {
            Flags = default(OperationResultFlags);
        }

        void IOperationResult.SetFlags(OperationResultFlags flags)
        {
            Flags = flags;
        }

        void IOperationResult.AppendFlags(OperationResultFlags flags)
        {
            Flags |= flags;
        }

        bool IOperationResult.Succeeded
        {
            get
            {
                return Succeeded;
            }
        }

        void IOperationResult.SetMessage(string message)
        {
            if (String.IsNullOrEmpty(Message))
            {
                Message = message;
            }
        }

    }
}
