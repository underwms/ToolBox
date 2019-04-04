using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public class ArgumentBindingAttribute : Attribute
    {
        public ArgumentBindingAttribute(string[] argumentNames) : this(null, argumentNames)
        {
        }

        public ArgumentBindingAttribute(string scopeName, string[] argumentNames)
        {
            if (null == argumentNames)
                throw new ArgumentNullException("argumentNames");

            ScopeName = ScopeName ?? KnownScopeName.Default;
            ArgumentNames = argumentNames;
        }

        public string ScopeName
        {
            get;
            private set;
        }

        public string[] ArgumentNames
        {
            get;
            private set;
        }
    }
}
