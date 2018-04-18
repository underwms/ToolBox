using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTests
{
    public class MapperDestination
    {
        private int _different = 0;

        public MapperDestination(int differentValue)
        {
            _different = differentValue;
        }

        public int MyProperty { get; set; }

        public int Different => _different;

        public SubDestiniation Sub { get; set; }
    }

    public class SubDestiniation
    {
        public int SubProperty { get; set; }
    }
}
