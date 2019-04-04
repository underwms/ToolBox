using System;

namespace WebApiExample.Models
{
    [Flags]
    public enum OperationResultFlags
    {
        Succeeded = 0x01,
        ExceptionOccurred = 0x02,
        Unknown = 0x04,
        InvalidContext = 0x08,
    }
}
