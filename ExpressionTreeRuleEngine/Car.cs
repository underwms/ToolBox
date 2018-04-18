using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeRuleEngine
{
    public class Car : ICar
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
