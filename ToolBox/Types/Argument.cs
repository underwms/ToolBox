using System;
using System.Text.RegularExpressions;

namespace ToolBox
{
    /// <summary>
    /// Value type that represents an argument from a command line.
    /// </summary>
    public struct Argument
    {
        /// <summary>
        /// Represents an empty argument value.
        /// </summary>
        public static readonly Argument Empty;

        /// <summary>
        /// Creates a new argument from the argument expression.
        /// </summary>
        /// <param name="expression"></param>
        public Argument(string expression)
        {

            // verify the expression string
            if (String.IsNullOrEmpty(expression))
                throw new ArgumentException("Cannot be null or zero-length string.", "expression");

            // split the expression using switch delimiters
            string[] __split = expression.Split(new char[] { ' ', ':', '=' }, 2);

            // assign member variables
            _name = __split[0].Replace("-", String.Empty).Replace("/", String.Empty);
            _value = (__split.Length == 2) ? __split[1].Trim() : String.Empty;
        }


        /// <summary>
        /// Creates a new argument with the specified name and value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Argument(string name, string value)
        {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException();

            _name = name;
            _value = (String.IsNullOrEmpty(value)) ? String.Empty : value;
        }


        /// <summary>
        /// Gets flag indicating whether the argument has a value.
        /// </summary>
        public bool HasValue
        {
            get { return (!String.IsNullOrEmpty(_value)); }
        }


        /// <summary>
        /// Gets flag indicating whether the argument is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Equals(Empty); }
        }


        private string _name;
        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                }
            }
        }


        /// <summary>
        /// Returns the string representation of the argument.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            string __toString = String.Format("{0}: {1}", Name, (HasValue) ? Value : "[none]");

            return __toString;
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string _value;
        /// <summary>
        /// Gets or sets the value of the argument.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                _value = (String.IsNullOrWhiteSpace(value)) ? String.Empty : value;
            }
        }


        /// <summary>
        /// Parse the argument string into an array of <see cref="Argument"/> values.
        /// </summary>
        /// <param name="args">The formatted argument string.</param>
        /// <returns>An array of <see cref="Argument"/> values.</returns>
        public static Argument[] Parse(string args)
        {

            // split the expression on recognized switch delimiters
            string[] __array = Regex.Split(args, @"\b\s[\-/]");

            // size an argument array
            Argument[] __Args = new Argument[__array.Length];

            // create arguments
            for (int i = 0; i < __array.Length; i++)
            {
                __Args[i] = new Argument(__array[i]);
            }

            // return the array of arguments
            return __Args;
        }
    }
}
