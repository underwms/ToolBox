using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    /// <summary>
    /// Static class of array-related methods.
    /// </summary>
    public static class ArrayUtility
    {
        /// <summary>
        /// Determines the index of a value within an array.
        /// </summary>
        /// <param name="array">A <see cref="System.Array"/> to search for the value.</param>
        /// <param name="value">A <see cref="System.Object"/> to locate in the array.</param>
        /// <returns>A <see cref="System.Int32"/> representing the integral index of the value in the array.  If the value does not exist in the array, -1 is returned.</returns>
        public static int IndexOf(Array array, object value)
        {
            int __index = -1;

            for (int i = 0; i < array.Length; i++)
            {
                if (array.GetValue(i).Equals(value))
                {
                    __index = i;
                    break;
                }
            }

            return __index;
        }
    }
}
