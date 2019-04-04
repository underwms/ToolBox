using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public static class ConversionUtility
    {
        /// <summary>
        /// Attempts to change the type of the specified value to the supplied value <see cref="Type"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type valueType)
        {
            return ChangeType(value, valueType, false);
        }

        /// <summary>
        /// Attempts to change the type of the specified value to the supplied value <see cref="Type"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <param name="useValueIfNotConverted"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type valueType, bool useValueIfNotConverted)
        {
            // default return value
            object result = null;

            // flag that indicates whether the item was successfully converted
            bool converted = false;

            try
            {
                // attempt to change the type of the value
                result = Convert.ChangeType(value, valueType);
                converted = true;
            }
            catch
            {
            }

            #region Pre-processing special cases

            if (!converted)
            {
                // check to see if the value type is an enumeration
                if (valueType.IsEnum)
                {
                    // parse the value as this enumeration type
                    result = EnumParser.Parse(valueType, value.ToString());
                    converted = true;
                }
            }

            #endregion

            // check to see if the item was successfully converted
            if (!converted)
            {
                // create a list of converters to be tried
                List<TypeConverter> converters = new List<TypeConverter>();

                // check for a TypeConverterAttribute on the type
                TypeConverterAttribute tca = ReflectionUtility.GetSingleAttribute<TypeConverterAttribute>(valueType);
                if (null != tca)
                {
                    // get the TypeConverter type
                    Type converterType = Type.GetType(tca.ConverterTypeName);
                    // check the reference
                    if (null != converterType)
                    {
                        // create an instance of the TypeConverter
                        TypeConverter converter = Activator.CreateInstance(converterType) as TypeConverter;
                        // verify the cast
                        if (null != converter)
                        {
                            // add this converter to the list
                            converters.Add(converter);
                        }
                    }
                }

                // check to see if the item is a nullable type
                Type type = Nullable.GetUnderlyingType(valueType);

                // determine whether an underlying type was found
                if (null != type)
                {
                    // determine the converter to use based on the underlying type
                    if (type == typeof(int))
                    {
                        converters.Add(new Int32Converter());
                    }
                    else if (type == typeof(decimal))
                    {
                        converters.Add(new DecimalConverter());
                    }
                    else if (type == typeof(bool))
                    {
                        converters.Add(new BooleanConverter());
                    }
                    else if (type == typeof(DateTime))
                    {
                        converters.Add(new DateTimeConverter());
                    }
                    else if (type == typeof(double))
                    {
                        converters.Add(new DoubleConverter());
                    }
                    else if (type == typeof(float))
                    {
                        converters.Add(new SingleConverter());
                    }
                    else if (type == typeof(long))
                    {
                        converters.Add(new Int64Converter());
                    }
                    else if (type == typeof(short))
                    {
                        converters.Add(new Int16Converter());
                    }
                    else if (type == typeof(byte))
                    {
                        converters.Add(new ByteConverter());
                    }
                    else if (type == typeof(uint))
                    {
                        converters.Add(new UInt32Converter());
                    }
                    else if (type == typeof(ushort))
                    {
                        converters.Add(new UInt16Converter());
                    }
                    else if (type == typeof(ulong))
                    {
                        converters.Add(new UInt64Converter());
                    }
                }
                else
                {
                    // add a list of converters to try in succession
                    converters.Add(new StringConverter());
                    converters.Add(new Int32Converter());
                    converters.Add(new DecimalConverter());
                    converters.Add(new BooleanConverter());
                    converters.Add(new DateTimeConverter());
                    converters.Add(new DoubleConverter());
                    converters.Add(new SingleConverter());
                    converters.Add(new Int64Converter());
                    converters.Add(new Int16Converter());
                    converters.Add(new ByteConverter());
                    converters.Add(new UInt32Converter());
                    converters.Add(new UInt16Converter());
                    converters.Add(new UInt64Converter());
                }

                // iterate each converter in the list
                foreach (var converter in converters)
                {
                    try
                    {
                        // attempt to convert the value using this converter
                        if (converter.CanConvertFrom(value.GetType()))
                        {
                            result = converter.ConvertFrom(value);
                            converted = true;
                        }

                        if (converted) break;
                    }
                    catch
                    {
                    }
                }

            }

            #region Post-processing special cases
            if (!converted)
            {
                // special cases
                //if ((value is IEnumerable) && (valueType == typeof(string)))
                //{
                //    value = new HPTS.Framework.Types.CommaSeparatedValue(value);
                //    result = value.ToString();
                //    converted = true;
                //}
                //else 
                if ((value is TimeSpan) && (valueType == typeof(string)))
                {
                    result = value.ToString();
                    converted = true;
                }
                //else if ((value is HPTS.Framework.Types.Date) && (valueType == typeof(string)))
                //{
                //    result = value.ToString();
                //    converted = true;
                //}
                //else if ((value is HPTS.Framework.Types.Date?) && (valueType == typeof(string)))
                //{
                //    result = value.ToString();
                //    converted = true;
                //}
                else if ((value is Guid) && (valueType == typeof(string)))
                {
                    result = value.ToString();
                    converted = true;
                }
                else if ((value is Guid?) && (valueType == typeof(string)))
                {
                    if (null != value)
                    {
                        result = value.ToString();
                    }
                    converted = true;
                }
                else if ((value is string) && ((valueType == typeof(Guid?)) || (valueType == typeof(Guid))))
                {
                    Guid parsed = default(Guid);
                    bool success = Guid.TryParse((string)value, out parsed);
                    if (success)
                    {
                        result = parsed;
                        converted = true;
                    }
                }
                else if ((value is System.Xml.Linq.XElement) && (valueType == typeof(string)))
                {
                    result = ((System.Xml.Linq.XElement)value).ToString();
                    converted = true;
                }
                else if (((value is DateTimeOffset) || (value is DateTimeOffset?)) && (valueType == typeof(string)))
                {
                    if (value is DateTimeOffset)
                    {
                        result = ((DateTimeOffset)value).DateTime.ToString();
                        converted = true;
                    }
                    else
                    {
                        if (null != value)
                        {
                            result = ((DateTimeOffset?)value).Value.DateTime.ToString();
                            converted = true;
                        }
                    }
                }
            }
            #endregion

            if ((!converted) && (useValueIfNotConverted))
            {
                result = value;
            }

            // return the result
            return result;
        }
    }
}
