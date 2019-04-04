using System;
using System.Reflection;

namespace ToolBox
{
    public enum NamingScheme
    {
        /// <summary>
        ///  The naming scheme is <b>Name</b>.
        /// </summary>
        Name,
        /// <summary>
        /// The naming scheme is <b>FriendlyName</b>.
        /// </summary>
        FriendlyName,
        /// <summary>
        /// The naming scheme is <b>Abbreviation</b>.
        /// </summary>
        Abbreviation,
        /// <summary>
        /// The naming scheme is <b>Any</b>.
        /// </summary>
        Any,
        /// <summary>
        /// The naming scheme is <b>ToolTip</b>.
        /// </summary>
        ToolTip
    }

    /// <summary>
    /// Class that wraps the task of resolving string values to enumerated constants without causing exceptions to be thrown when invalid values are specified.
    /// </summary>
    /// <remarks>This class is sealed.</remarks>
    public sealed class EnumParser
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of EnumParser.
        /// </summary>
        /// <remarks>
        /// This is declared as private to prevent class instantiation.
        /// </remarks>
        private EnumParser()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicator of whether or not to strictly enforce parsed names.
        /// </summary>
        /// <remarks>
        /// The default value is false.
        /// </remarks>
        public static readonly bool OptionStrict;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Converts the string or numeric representation of one or more enumerated constants to an equivalent enumerated object.
        /// This method first checks to see if the value is contained in the array of field
        /// names for the supplied enumeration type.  If not, it checks each field for a 
        /// <see cref="NamingAttribute"/> declaration where the abbreviation or the 
        /// friendly name matches the value.
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration.</param>
        /// <param name="value">The <see cref="System.String"/> value to parse.</param>
        /// <param name="ignoreCase">A <see cref="System.Boolean"/> indicating whether or not to ignore case.</param>
        /// <returns>A <see cref="System.Enum"/> constant value.</returns>
        public static Enum Parse(Type enumType, String value, Boolean ignoreCase)
        {
            if (null == enumType)
                throw new ArgumentNullException("enumType");

            // declare return value
            Enum __enum = null;

            bool __found = false;

            try
            {
                // check to see if the value is null or empty
                if (String.IsNullOrEmpty(value))
                {
                    // parse as the default (0 value) item
                    __enum = (Enum)Enum.Parse(enumType, "0");
                    __found = true;
                }
                else
                {
                    // try to resolve the value
                    string[] __names = Enum.GetNames(enumType);

                    if (StringHelper.EqualsAny(value, __names))
                    {
                        // try to parse the enumeration using the built-in parsing mechanism
                        __enum = (Enum)Enum.Parse(enumType, value, ignoreCase);
                        __found = true;
                    }
                    else
                    {
                        // get the fields for the enumeration type
                        FieldInfo[] __Fields = enumType.GetFields();
                        // iterate each field in the array
                        foreach (FieldInfo __FieldInfo in __Fields)
                        {
                            // get any NamingAttribute declaration for this field
                            object[] __Attributes = __FieldInfo.GetCustomAttributes(typeof(NamingAttribute), false);
                            // see if only one was found
                            if (__Attributes.Length == 1)
                            {
                                // get the attribute
                                NamingAttribute __NamingAttribute = __Attributes[0] as NamingAttribute;
                                // check to see if the value matches either the abbreviation or the friendly name
                                if (StringHelper.EqualsAny(value, ignoreCase,
                                    __NamingAttribute.Abbreviation,
                                    __NamingAttribute.FriendlyName))
                                {
                                    // get this field for the return value
                                    __enum = (Enum)Enum.Parse(enumType, __FieldInfo.Name, true);
                                    __found = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                // see if the value was found
                if (!__found)
                {
                    // force the argument exception
                    __enum = (Enum)Enum.Parse(enumType, value, ignoreCase);
                }

            }
            catch (ArgumentException __ArgumentException)
            { // catch when the request value does not exist

                // check to see if OptionStrict is on
                if (OptionStrict)
                    throw __ArgumentException; // rethrow the exception

                if (!__found)
                    // create an instance for the default value (integral value of 0)
                    __enum = (Enum)Activator.CreateInstance(enumType);

            }
            catch
            {
                throw;
            }

            // return the parsed value
            return __enum;
        }


        /// <summary>
        /// Converts the string or numeric representation of one or more
        /// enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration.</param>
        /// <param name="value">The <see cref="System.String"/> value to parse.</param>
        /// <returns>A <see cref="System.Enum"/> constant value.</returns>
        public static Enum Parse(Type enumType, String value)
        {
            // forward the call to the overload
            return Parse(enumType, value, false);
        }


        /// <summary>
        /// Retrieves the name for the specified enumeration type and value
        /// using the default naming scheme of "FriendlyName". 
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration.</param>
        /// <param name="value">The <see cref="System.Object"/> value to retrieve the name.</param>
        /// <returns>A <see cref="System.String"/> representing the name of the value.</returns>
        public static String GetName(Type enumType, object value)
        {
            return GetName(enumType, value, NamingScheme.FriendlyName);
        }


        /// <summary>
        /// Retrieves the name for the specified enumeration type, value, and 
        /// naming scheme. 
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration.</param>
        /// <param name="value">The <see cref="System.Object"/> value to retrieve the name.</param>
        /// <param name="namingScheme">The <see cref="NamingScheme"/> to use in retrieving the name.</param>
        /// <returns>A <see cref="System.String"/> representing the name of the value.</returns>
        public static String GetName(Type enumType, object value, NamingScheme namingScheme)
        {

            if (null == enumType)
                throw new ArgumentNullException("enumType");

            if (null == value)
                throw new ArgumentNullException("value");

            // declare return value
            String __name = null;
            // get the array of names 
            String[] __names1 = GetNames(enumType, NamingScheme.Name);
            // get the index of the value within the actual names array
            int __index = ArrayUtility.IndexOf(__names1, value.ToString());
            // check the index
            if (__index != -1)
            {
                // get the array of names using the supplied naming scheme
                String[] __names2 = GetNames(enumType, namingScheme);
                // retrieve the name based on the index determined above
                __name = __names2[__index];
            }
            // return the name
            return __name;
        }


        /// <summary>
        /// Static function to retrieve the names for the specified enumeration type 
        /// using the default naming scheme of "FriendlyName". 
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration.</param>
        /// <returns>An array of <see cref="System.String"/> representing the names.</returns>
        public static String[] GetNames(Type enumType)
        {
            return GetNames(enumType, NamingScheme.FriendlyName);
        }


        /// <summary>
        /// Retrieves the names for the specified enumeration type using the
        /// specified naming scheme. 
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration.</param>
        /// <param name="namingScheme">The <see cref="NamingScheme"/> to use in retrieving the names.</param>
        /// <returns>An array of <see cref="System.String"/> representing the names.</returns>
        public static String[] GetNames(Type enumType, NamingScheme namingScheme)
        {

            if (null == enumType)
                throw new ArgumentNullException("enumType");

            // default return value
            String[] __names = null;
            // get the fields for this type
            FieldInfo[] __Fields = enumType.GetFields();
            // dimension the array based on the field length minus one (for "value__" which is omitted)
            __names = new String[__Fields.Length - 1];
            // iterate the field array
            for (Int32 i = 0, j = 0; i < __Fields.Length; i++)
            {
                // get the current field info
                FieldInfo __FieldInfo = __Fields[i];
                // get any NamingAttribute declarations for the field
                object[] __Attributes = __FieldInfo.GetCustomAttributes(typeof(NamingAttribute), false);
                // see if one (and only one) was found
                if (__Attributes.Length == 1)
                {
                    // get the NamingAttribute
                    NamingAttribute __NamingAttribute = __Attributes[0] as NamingAttribute;
                    // declare string value
                    String __value = null;
                    // switch the naming scheme
                    switch (namingScheme)
                    {
                        case NamingScheme.Name:
                            // get the actual name
                            __value = __FieldInfo.Name;
                            break;
                        case NamingScheme.FriendlyName:
                            // get the first matching value - friendly name or name
                            __value = __NamingAttribute.FriendlyName ?? __FieldInfo.Name;
                            break;
                        case NamingScheme.Abbreviation:
                            // get the first matching value - abbreviation or name
                            __value = __NamingAttribute.Abbreviation ?? __FieldInfo.Name;
                            break;
                        case NamingScheme.ToolTip:
                            // get the first matching value - friendly name or name
                            __value = __NamingAttribute.ToolTip ?? __FieldInfo.Name;
                            break;
                        case NamingScheme.Any:
                        default:
                            // get the first matching value - friendlyname, abbreviation or name
                            __value = __NamingAttribute.FriendlyName ?? __NamingAttribute.Abbreviation ?? __FieldInfo.Name;
                            break;

                    }
                    // assign the value to the current index
                    __names[j++] = __value;
                }
                else
                {
                    // ignore the value__ field
                    if (__FieldInfo.Name != "value__")
                        __names[j++] = __FieldInfo.Name;
                }
            }
            // return the names array
            return __names;
        }

        #endregion Methods
    }
}
