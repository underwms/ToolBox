using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTests
{
    public interface IACIRequestEntity
    {
        string XmlNameSpace { get; }
        Identity Identity { get; set; }
    }
}
