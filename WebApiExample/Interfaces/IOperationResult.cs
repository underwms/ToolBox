using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiExample.Models;

namespace WebApiExample.Interfaces
{
    public interface IOperationResult
    {
        void ClearFlags();

        void SetFlags(OperationResultFlags flags);

        void AppendFlags(OperationResultFlags flags);

        bool Succeeded
        {
            get;
        }

        void SetMessage(string message);
    }
}
