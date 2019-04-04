using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public class StringHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int HashIter = 10000;
        
        public const string Comma = ",";
        public const string Quote = "\"";
        public const string Period = ".";
        public const string Pipe = "|";
        public const string SpaceChar = " ";
        public const string Zero = "0";
        public const string Null = "null";
        public const string Unknown = "(unknown)";
        public const string UnknownShort = "?";
        public const string Present = "present";
        public const string AM = "AM";
        public const string PM = "PM";
        public const string GreaterThan = ">";
        public const string LeftBrace = "{";
        public const string RightBrace = "}";
        public const string Backslash = "\\";
        
        private readonly byte[] _salt;
        private readonly byte[] _hash;
        
        public byte[] Salt => (byte[])_salt.Clone();
        public byte[] Hash => (byte[])_hash.Clone();

        public StringHelper(string password)
        {
            if (string.IsNullOrEmpty(password)) return;

            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }

        public StringHelper(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }

        public StringHelper(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }
        
        public byte[] ToArray()
        {
            var hashBytes = new byte[SaltSize + HashSize];

            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);

            return hashBytes;
        }

        public bool Verify(string password)
        {
            var test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (var i = 0; i < HashSize; i++)
            {
                if (test[i] != _hash[i])
                { return false; }
            }

            return true;
        }

        public string RandomString(int take)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[take];
                crypto.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(take);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length)]); }

            return result.ToString();
        }

        /// <summary>
        /// Appends the specified string to the target string using 
        /// <see cref="String.Empty"/> as the separator.
        /// </summary>
        /// <param name="target">A <see cref="System.String"/> representing the target string.</param>
        /// <param name="append">A <see cref="System.String"/> representing the string to append to the target.</param>
        /// <returns>A <see cref="System.String"/> representing the target string with the appended string included.</returns>
        public static string AppendString(string target, string append)
        {
            return AppendString(target, String.Empty, append);
        }

        /// <summary>
        /// Appends the specified string to the target string using the 
        /// supplied separator when the target is not null or zero-length.
        /// </summary>
        /// <param name="target">A <see cref="System.String"/> representing the target string.</param>
        /// <param name="append">A <see cref="System.String"/> representing the string to append to the target.</param>
        /// <param name="delim">A <see cref="System.String"/> representing the string to delimiter used in parsing the string to append.</param>
        /// <returns>A <see cref="System.String"/> representing the target string with the appended string included.</returns>
        public static string AppendString(string target, string delim, string append)
        {
            return AppendString(target, append, delim, false);
        }

        /// <summary>
        /// Appends the specified string to the target string using the 
        /// supplied separator when the target is not null or zero-length.
        /// </summary>
        /// <param name="target">A <see cref="System.String"/> representing the target string.</param>
        /// <param name="delim">A <see cref="System.String"/> representing the string to delimiter used in parsing the string to append.</param>
        /// <param name="values">An array of <see cref="System.String"/> that is to be appended to the target string.</param>
        /// <returns>A <see cref="System.String"/> representing the target string with the appended string included.</returns>
        public static string AppendString(string target, string delim, params string[] values)
        {
            string result = target;
            foreach (string value in values)
            {
                result = AppendString(result, value, delim);
            }
            return result;
        }

        /// <summary>
        /// Implements the string appending / prepending logic.
        /// </summary>
        /// <param name="target">A <see cref="System.String"/> representing the target string.</param>
        /// <param name="append">A <see cref="System.String"/> representing the string to append to the target.</param>
        /// <param name="delim">A <see cref="System.String"/> representing the string to delimiter used in parsing the string to append.</param>
        /// <param name="prepend">A <see cref="System.Boolean"/> indicator of whether to apply the string to the end of the beginning of the target string.</param>
        /// <returns></returns>
        private static string AppendString(string target, string append, string delim, bool prepend)
        {
            string result = null;

            if (!String.IsNullOrEmpty(append))
            {
                if (String.IsNullOrEmpty(target)) delim = String.Empty;

                if (prepend)
                {
                    result = append + delim + target;
                }
                else
                {
                    result = target + delim + append;
                }
            }
            else
            {
                result = target;
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified string values are equal (case-sensitive).
        /// </summary>
        /// <param name="string1">The <see cref="System.String"/> to compare with.</param>
        /// <param name="string2">The <see cref="System.String"/> to compare to.</param>
        /// <returns>A <see cref="System.Boolean"/> indicator of whether the strings are equal.</returns>
        public static bool AreEqual(string string1, string string2)
        {
            return AreEqual(string1, string2, false);
        }

        /// <summary>
        /// Determines whether the specified string values are equal 
        /// optionally ignoring the case.
        /// </summary>
        /// <param name="string1">The <see cref="System.String"/> to compare with.</param>
        /// <param name="string2">The <see cref="System.String"/> to compare to.</param>
        /// <param name="ignoreCase">A <see cref="System.Boolean"/> indicator of whether to ignore the case of the strings when comparing them.</param>
        /// <returns>A <see cref="System.Boolean"/> indicator of whether the strings are equal.</returns>
        public static bool AreEqual(string string1, string string2, bool ignoreCase)
        {
            return EqualsAny(string1, ignoreCase, string2);
        }

        /// <summary>
        /// Retrieves a camel-cased version of the supplied string value.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to convert to camel-case.</param>
        /// <returns>A <see cref="System.String"/> in camel-cased format.</returns>
        public static string CamelCase(string value)
        {
            // default return value
            string result = null;
            // check the supplied string
            if (!string.IsNullOrEmpty(value))
            {
                // camel case the value
                result = StringHelper.Left(value, 1).ToLower() + StringHelper.Right(value, value.Length - 1);
            }
            else
            {
                // echo the value
                result = value;
            }
            // return the result
            return result;
        }

        /// <summary>
        /// Retrieves a version of the supplied string value with the first letter capitalized.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to capitalize.</param>
        /// <returns>A <see cref="System.String"/> with the first letter capitalized.</returns>
        public static string CapitalizeFirstLetter(string value)
        {
            // default return value
            string result = null;
            // check the supplied string
            if (!string.IsNullOrEmpty(value))
            {
                // Capitalize the value
                result = StringHelper.Left(value, 1).ToUpper() + StringHelper.Right(value, value.Length - 1);
            }
            else
            {
                // echo the value
                result = value;
            }
            // return the result
            return result;
        }

        /// <summary>
        /// Determines whether the source string contains the
        /// specified value.
        /// </summary>
        /// <param name="source">The <see cref="System.String"/> used to determine if it contains the value.</param>
        /// <param name="value">The <see cref="System.String"/> used to search the source string.</param>
        /// <returns>A <see cref="System.Boolean"/> indicator of whether the value was found in the source string.</returns>
        public static bool Contains(string source, string value)
        {
            bool contains = (StringHelper.IndexOf(source, value) != -1);
            return contains;
        }

        /// <summary>
        /// Retrieves the first non-null or non-empty string supplied in the array
        /// of arguments.
        /// </summary>
        /// <param name="values">The array of string values to coalesce.</param>
        /// <returns>The first string meeting the criteria; otherwise null.</returns>
        public static string Coalesce(params string[] values)
        {
            return Coalesce(false, values);
        }

        /// <summary>
        /// Retrieves the first non-null (or optionally non-empty) string supplied in
        /// the array of arguments.
        /// </summary>
        /// <param name="allowEmpty">Flag that indicates whether to allow zero-length strings to be returned.</param>
        /// <param name="values">The array of string values to coalesce.</param>
        /// <returns>The first string meeting the criteria; otherwise null.</returns>
        public static string Coalesce(bool allowEmpty, params string[] values)
        {
            // default return value
            string result = null;

            // verify the array of values
            if (null != values)
            {
                // iterate the array
                for (int i = 0; i < values.Length; i++)
                {
                    // set flag based on the allow empty parameter
                    bool flag = (allowEmpty) ? (null != values[i]) : (!String.IsNullOrEmpty(values[i]));
                    // check to see if the criteria was met
                    if (flag)
                    {
                        // assign the result and break out
                        result = values[i];
                        break;
                    }
                }
            }

            // return the result
            return result;
        }

        /// <summary>
        /// Retrieves an indicator of whether the specified string value equals
        /// any of the strings in the supplied array.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="value">The <see cref="System.String"/> to compare to the array.</param>
        /// <param name="values">The array of string values to determine if the value is present.</param>
        /// <returns>A <see cref="System.Boolean"/> indicator of whether the value was found in the array.</returns>
        public static bool EqualsAny(string value, params string[] values)
        {
            return EqualsAny(value, false, values);
        }

        /// <summary>
        /// Retrieves an indicator of whether the specified string value equals
        /// any of the strings in the supplied array optionally ignoring case.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to compare to the array.</param>
        /// <param name="ignoreCase">A <see cref="System.Boolean"/> indicator of whether to ignore the case of the strings when comparing them.</param>
        /// <param name="values">The array of string values to determine if the value is present.</param>
        /// <returns>A <see cref="System.Boolean"/> indicator of whether the value was found in the array.</returns>       
        public static bool EqualsAny(string value, bool ignoreCase, params string[] values)
        {
            if (null == values)
                throw new ArgumentNullException("values");

            // default pessimistic
            bool result = false;
            if (null != values)
            {
                // iterate the array of values
                for (int i = 0; i < values.Length; i++)
                {
                    // compare the string values using the case flag
                    if (string.Compare(value, values[i], ignoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            // return the result
            return result;
        }

        /// <summary>
        /// Retrieves a string from the specified value using the specified fallback
        /// value if the string is null or zero-length.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to retrieve.</param>
        /// <param name="fallback">The <see cref="System.String"/> representing the fallback value.</param>
        /// <returns>A <see cref="System.String"/> representing the result of the string retrieval.</returns>
        public static string GetString(string value, string fallback)
        {
            return (!String.IsNullOrEmpty(value)) ? value : fallback;
        }

        /// <summary>
        /// Retrieves a string from the specified value using <see cref="String.Empty"/>
        /// as the fallback value if the string is null or zero-length.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to retrieve.</param>
        /// <returns>A <see cref="System.String"/> representing the result of the string retrieval.</returns>
        public static string GetString(string value)
        {
            return GetString(value, String.Empty);
        }

        /// <summary>
        /// Retrieves a string from the specified value using the specified fallback
        /// value if the string is null or zero-length.
        /// </summary>
        /// <param name="value">The value to retrieve.</param>
        /// <param name="fallback">The <see cref="System.String"/> representing the fallback value.</param>
        /// <returns>A <see cref="System.String"/> representing the result of the string retrieval.</returns>
        public static string GetString(object value, string fallback)
        {
            return (null != value) ? GetString(value.ToString(), fallback) : fallback;
        }

        /// <summary>
        /// Retrieves a string from the specified value using <see cref="String.Empty"/>
        /// as the fallback value if the string is null or zero-length.
        /// </summary>
        /// <param name="value">The value to retrieve.</param>
        /// <returns>A <see cref="System.String"/> representing the result of the string retrieval.</returns>
        public static string GetString(object value)
        {
            return GetString(value, String.Empty);
        }

        /// <summary>
        /// Determines if a value is null.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to determine if it is null.</param>
        /// <returns>A <see cref="System.Boolean"/> indicating whether the value is null.</returns>
        public static bool IsNull(string value)
        {
            return (null == value);
        }

        /// <summary>
        /// Determines whether the supplied string represents a numeric value.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to determine if numeric.</param>
        /// <returns>A <see cref="System.Boolean"/> indicating whether the string is numeric.</returns>
        public static bool IsNumeric(string value)
        {
            // declare a decimal
            decimal parsed = 0.0m;
            // try parsing the string to a decimal value
            bool result = decimal.TryParse(value, out parsed);
            // return the result
            return result;
        }

        /// <summary>
        /// Limits the supplied string to the specified length.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to limit in size.</param>
        /// <param name="length">The <see cref="System.Int32"/> representing the size to limit the value.</param>
        /// <returns>A <see cref="System.String"/> representing the string sized to its limit.</returns>
        public static string Limit(string value, int length)
        {
            return Limit(value, length, String.Empty);
        }

        /// <summary>
        /// Limits the supplied string to the specified length and displays
        /// the specified trailer string as the continuation indicator.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="value">The <see cref="System.String"/> representing the value to limit in size.</param>
        /// <param name="length">The <see cref="System.Int32"/> representing the size to limit the value.</param>
        /// <param name="trailer">The <see cref="System.String"/> representing the trailer characters.</param>
        /// <returns>A <see cref="System.String"/> representing the string sized to its limit.</returns>
        public static string Limit(string value, int length, string trailer)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "Length must be greater than or equal to zero.");

            // default return value
            string result = null;

            // verify the value
            if (!String.IsNullOrEmpty(value))
            {
                if (value.Length < length)
                {
                    result = value;
                }
                else
                {
                    // determine the target substring length
                    int len = length - trailer.Length;
                    // adjust length when negative
                    if (len < 0) len = 0;
                    // get the substring value
                    result = value.Substring(0, len) + trailer;
                }
            }

            // return the result
            return result;
        }

        /// <summary>
        /// Normalizes a delimited string value to eliminate duplicates based on 
        /// the default character separator <see cref="Characters.Comma"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to normalize.</param>
        /// <returns>A <see cref="System.String"/> representing the normalized string.</returns>
        public static string Normalize(string value)
        {
            return Normalize(value, ',');
        }

        /// <summary>
        /// Normalizes a delimited string value to eliminate duplicates based on 
        /// the supplied character separator.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> representing the value to normalize.</param>
        /// <param name="separator">The <see cref="System.Char"/> to use as the separator.</param>
        /// <returns>A <see cref="System.String"/> representing the normalized string.</returns>
        public static string Normalize(string value, char separator)
        {
            // declare the result
            string result = null;
            // validate the string value
            if (!String.IsNullOrEmpty(value))
            {
                // split the value using the supplied character separator;
                // get the distinct array of values from the source array
                string[] split = value.Split(separator).Distinct().ToArray();
                // create a string array
                string[] values = new string[split.Length];
                // copy the values to the string array
                split.CopyTo(values, 0);
                // rejoin the string using the supplied character separator
                result = string.Join(Char.ToString(separator), values);
            }
            else
            {
                // assign result as the supplied value
                result = value;
            }
            // return the normalized string
            return result;
        }

        /// <summary>
        /// Prepends the specified string to the target string using 
        /// <see cref="String.Empty"/> as the separator.
        /// </summary>
        /// <param name="target">A <see cref="System.String"/> representing the target string.</param>
        /// <param name="append">A <see cref="System.String"/> representing the string to prepend to the target.</param>
        /// <returns>A <see cref="System.String"/> representing the target string with the prepended string included.</returns>
        public static string PrependString(string target, string append)
        {
            return PrependString(target, append, String.Empty);
        }

        /// <summary>
        /// Prepends the specified string to the target string using 
        /// the supplied separator when the target string is not null or empty.
        /// </summary>
        /// <param name="target">A <see cref="System.String"/> representing the target string.</param>
        /// <param name="append">A <see cref="System.String"/> representing the string to prepend to the target.</param>
        /// <param name="delim">A <see cref="System.String"/> representing the string to delimiter used in parsing the string to prepend.</param>
        /// <returns>A <see cref="System.String"/> representing the target string with the prepended string included.</returns>
        public static string PrependString(string target, string append, string delim)
        {
            return AppendString(target, append, delim, true);
        }

        /// <summary>
        /// Repeats the supplied string value for the specified count.
        /// </summary>
        /// <param name="value">The value to repeat.</param>
        /// <param name="count">The number of times to repeat the value.</param>
        /// <returns>The repeated string value.</returns>
        public static string Repeat(string value, int count)
        {
            string retval = null;

            // verify that the string has length and the count is greater than zero
            if ((!String.IsNullOrEmpty(value)) && (count > 0))
            {

                // check to see if the count is one
                if (count == 1)
                {
                    // assign the value
                    retval = value;
                }
                else
                {
                    // create a string buffer of the specified count
                    string[] buffer = new string[count];
                    // populate the buffer with the value
                    for (int i = 0; i < count; i++)
                    {
                        buffer[i] = value;
                    }
                    // join the array values into a single string
                    retval = string.Join(String.Empty, buffer);
                }
            }

            if (null == retval)
                retval = String.Empty;

            return retval;
        }

        /// <summary>
        /// Replaces all occurrences of strings in the supplied array with the specified
        /// replacement string.
        /// </summary>
        /// <param name="target">The <see cref="System.String"/> to be replaced in the array of strings.</param>
        /// <param name="replacement">The <see cref="System.String"/> to replace with in the array of strings.</param>
        /// <param name="values">The array of strings to search and replace with the target and replacement values.</param>
        /// <returns>A <see cref="System.String"/> containing the replaced values.</returns>
        public static string ReplaceAll(string target, string replacement, params string[] values)
        {
            string result = target;

            if (!String.IsNullOrEmpty(target))
            {
                if (null != values)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        result = result.Replace(values[i], replacement);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a fixed width string of the specified character width.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> value to retrieve the fixed width string.</param>
        /// <param name="width">The <see cref="System.Int32"/> representing the number of characters to fix the string.</param>
        /// <returns>A <see cref="System.String"/> with a fixed width.</returns>       
        public static string FixedWidth(string value, int width)
        {
            return FixedWidth(value, width, true);
        }

        /// <summary>
        /// Retrieves a fixed width string of the specified character width and alignment.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> value to retrieve the fixed width string.</param>
        /// <param name="width">The <see cref="System.Int32"/> representing the number of characters to fix the string.</param>
        /// <param name="leftAligned">A <see cref="System.Boolean"/> indicator of whether to left align the string.</param>
        /// <returns>A <see cref="System.String"/> with a fixed width.</returns>       
        public static string FixedWidth(string value, int width, bool leftAligned)
        {
            return FixedWidth(value, width, Space(1), leftAligned);
        }

        /// <summary>
        /// Retrieves a fixed width string of the specified character width using the
        /// supplied padding string.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> value to retrieve the fixed width string.</param>
        /// <param name="width">The <see cref="System.Int32"/> representing the number of characters to fix the string.</param>
        /// <param name="padding">The <see cref="System.String"/> representing the string used to pad.</param>
        /// <returns>A <see cref="System.String"/> with a fixed width.</returns>       
        public static string FixedWidth(string value, int width, string padding)
        {
            return FixedWidth(value, width, padding, true);
        }

        /// <summary>
        /// Retrieves a fixed width string of the specified character width using the
        /// supplied padding string and alignment.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> value to retrieve the fixed width string.</param>
        /// <param name="width">The <see cref="System.Int32"/> representing the number of characters to fix the string.</param>
        /// <param name="padding">The <see cref="System.String"/> representing the string used to pad.</param>
        /// <param name="leftAligned">A <see cref="System.Boolean"/> indicator of whether to left align the string.</param>
        /// <returns>A <see cref="System.String"/> with a fixed width.</returns>       
        public static string FixedWidth(string value, int width, string padding, bool leftAligned)
        {
            if (String.IsNullOrEmpty(padding))
                throw new ArgumentNullException("padding");

            string retval = null;

            value = GetString(value);

            if (width > 0)
            {
                if (value.Length > width)
                {
                    //throw new InvalidOperationException("String data would be truncated.");
                    value = Limit(value, width);
                }
                //else
                {
                    retval = Pad(value, width - value.Length, padding, leftAligned);
                }
            }

            return retval;
        }

        /// <summary>
        /// Retrieves a string with the specified count of spaces.
        /// </summary>
        /// <param name="count">The <see cref="System.Int32"/> representing the number of spaces to include in the string.</param>
        /// <returns>A <see cref="System.String"/> with a specified number of spaces.</returns>       
        public static string Space(int count)
        {
            return Repeat(" ", count);
        }

        /// <summary>
        /// Determines whether the specified string starts with
        /// any of the strings in the supplied array.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to find at the start of a value in the array.</param>
        /// <param name="values">The array of strings to search for the specified value.</param>
        /// <returns>A <see cref="System.Boolean"/> indicating whether the string starts with the specified value.</returns>
        public static bool StartsWithAny(string value, params string[] values)
        {
            return StartsWithAny(value, false, values);
        }

        /// <summary>
        /// Determines whether the specified string value starts with
        /// any of the strings in the supplied array optionally ignoring case.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to find at the start of a value in the array.</param>
        /// <param name="ignoreCase">A <see cref="System.Boolean"/> indicator of whether to ignore the case of the strings when comparing them.</param>
        /// <param name="values">The array of strings to search for the specified value.</param>
        /// <returns>A <see cref="System.Boolean"/> indicating whether the string starts with the specified value.</returns>
        public static bool StartsWithAny(string value, bool ignoreCase, params string[] values)
        {
            if (null == value) return false;

            // default pessimistic
            bool result = false;
            if (ignoreCase) value = value.ToLower();
            if (null != values)
            {
                // iterate the array of values
                for (int i = 0; i < values.Length; i++)
                {
                    if (!String.IsNullOrEmpty(values[i]))
                    {
                        string segment = (ignoreCase) ? values[i].ToLower() : values[i];
                        // compare the string values using the case flag
                        if (value.StartsWith(segment))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            // return the result
            return result;
        }

        /// <summary>
        /// Pads the supplied string value with the specified number of spaces.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to pad.</param>
        /// <param name="count">The <see cref="System.Int32"/> representing the number of spaces to pad.</param>
        /// <returns>A <see cref="System.String"/> with a specified number of spaces padded.</returns>       
        public static string Pad(string value, int count)
        {
            return Pad(value, count, true);
        }

        /// <summary>
        /// Pads a string with the specified number of spaces and alignment designation.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to pad.</param>
        /// <param name="count">The <see cref="System.Int32"/> representing the number of spaces to pad.</param>
        /// <param name="leftAligned">A <see cref="System.Boolean"/> indicator of whether to left align the string.</param>
        /// <returns>A <see cref="System.String"/> with a specified number of spaces padded.</returns>       
        public static string Pad(string value, int count, bool leftAligned)
        {
            return Pad(value, count, Space(1), leftAligned);
        }

        /// <summary>
        /// Pads a string with the specified number of spaces, padding string and alignment.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to pad.</param>
        /// <param name="count">The <see cref="System.Int32"/> representing the number of spaces to pad.</param>
        /// <param name="padding">The <see cref="System.String"/> representing the string used to pad.</param>
        /// <param name="leftAligned">A <see cref="System.Boolean"/> indicator of whether to left align the string.</param>
        /// <returns>A <see cref="System.String"/> with a specified number of spaces padded.</returns>       
        public static string Pad(string value, int count, string padding, bool leftAligned)
        {
            string retval = null;

            retval = GetString(value);

            if (count > 0)
            {

                retval = (leftAligned) ? value + Repeat(padding, count) : Repeat(padding, count) + value;
            }

            return retval;
        }

        /// <summary>
        /// Returns the leftmost characters in a string of the specified length.
        /// </summary>
        /// <param name="param">The <see cref="System.String"/> to get the leftmost characters.</param>
        /// <param name="length">The <see cref="System.Int32"/> representing the number of characters to extract from the left portion of the string.</param>
        /// <returns>A <see cref="System.String"/> with a specified number of leftmost characters.</returns>
        public static string Left(string param, int length)
        {
            // default return value
            string result = null;

            // check if the length of the string passed is more than or equal to length
            if ((!String.IsNullOrEmpty(param)) && (param.Length > length))
            {
                result = param.Substring(0, length);
            }
            else
            {
                result = param;
            }

            // return the result
            return result;
        }

        /// <summary>
        /// Returns the rightmost characters in a string of the specified length.
        /// </summary>
        /// <param name="param">The <see cref="System.String"/> to get the right-most characters.</param>
        /// <param name="length">The <see cref="System.Int32"/> representing the number of characters to extract from the right portion of the string.</param>
        /// <returns>A <see cref="System.String"/> with a specified number of right-most characters.</returns>
        public static string Right(string param, int length)
        {
            // default return value
            string result = null;

            // check if the length of the string passed is more than or equal to length
            if ((!String.IsNullOrEmpty(param)) && (param.Length > length))
            {
                result = param.Substring(param.Length - length, length);
            }
            else
            {
                result = param;
            }

            // return the result
            return result;
        }

        /// <summary>
        /// Returns the starting position of a string within the specified string.
        /// </summary>
        /// <param name="source">The string to search.</param>
        /// <param name="search">The string to search for.</param>
        /// <returns>A <see cref="System.Int32"/> representing the starting position of the character located in the string.</returns>
        public static int IndexOf(string source, string search)
        {
            // default return value
            int result = -1;

            // verify the source string
            if (!String.IsNullOrEmpty(source))
            {
                result = source.IndexOf(search);
            }

            // return the position
            return result;
        }

        /// <summary>
        /// Removes non-numeric characters from a string, such as a phone number.
        /// </summary>
        /// <param name="input">string containing numeric characters to isolate.</param>
        /// <returns>A string containing only numbers.</returns>
        public static string StripNonNumeric(string input)
        {
            string result = null;

            if (!String.IsNullOrEmpty(input))
            {
                StringBuilder sb = new StringBuilder();
                foreach (char character in input)
                {
                    if (Char.IsNumber(character))
                        sb.Append(character);
                }
                result = sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// Parses a delimited string value using default character separators
        /// into a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to parse into a name value collection.</param>
        /// <returns>A <see cref="NameValueCollection"/> containing the parsed strings.</returns>
        public static NameValueCollection ToNameValueCollection(string value)
        {
            return ToNameValueCollection(value, ',', '=');
        }

        /// <summary>
        /// Parses a delimited string value using the specified character separators
        /// into a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.String"/> to parse into a name value collection.</param>
        /// <param name="pairDelimiter">The <see cref="System.Char"/> representing the pair delimiter.</param>
        /// <param name="itemDelimiter">The <see cref="System.Char"/> representing the item delimiter.</param>
        /// <returns>A <see cref="NameValueCollection"/> containing the parsed strings.</returns>
        public static NameValueCollection ToNameValueCollection(string value, char pairDelimiter, char itemDelimiter)
        {
            NameValueCollection result = new NameValueCollection();
            if (null != value)
            {
                string[] pairs = value.Split(pairDelimiter);
                for (int i = 0; i < pairs.Length; i++)
                {
                    string[] pair = pairs[i].Split(itemDelimiter);
                    string theName = pair[0];
                    string theValue = (pair.Length > 1) ? pair[1] : String.Empty;
                    result.Add(theName, theValue);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns flag indicating whether all the supplied strings are not null or empty.
        /// </summary>
        /// <param name="values">The values to check.</param>
        /// <returns>True if no value is null or empty; otherwise false.</returns>
        public static bool NoneNullOrEmpty(params string[] values)
        {
            // default optimistic return value
            bool result = true;

            // make sure the array is valid
            if (null != values)
            {
                // iterate each string in the array
                foreach (string value in values)
                {
                    // check the value for null or empty
                    if (String.IsNullOrEmpty(value))
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                // toggle result
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Returns flag indicating whether all values are specified (e.g. they are all non-null and non-empty) or
        /// all values are not specified (e.g. they are all null or empty).
        /// </summary>
        /// <param name="values">The array of string values to check.</param>
        /// <returns>True if all values are specified or all not specified; false if any are mis-matched.</returns>
        public static bool AllOrNoneSpecified(params string[] values)
        {
            bool result = true;

            if (null != values)
            {
                bool yes = false;
                bool no = false;

                int counter = 0;
                foreach (string value in values)
                {
                    if (String.IsNullOrEmpty(value))
                    {
                        no = true;
                    }
                    else
                    {
                        yes = true;
                    }

                    if (counter > 0)
                    {
                        if ((yes) && (no))
                        {
                            result = false;
                            break;
                        }
                    }

                    counter++;
                }
            }

            return result;
        }

        /// <summary>
        /// Splits a delimited attribute string value using the standard
        /// set of delimiting characters.
        /// </summary>
        /// <remarks>
        /// Splits the value using comma, semi-colon and pipe characters.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] SplitDelimitedAttributeValue(string value)
        {
            string[] result = null;

            if (!String.IsNullOrEmpty(value))
            {
                result = value.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return result;
        }
    }
}
