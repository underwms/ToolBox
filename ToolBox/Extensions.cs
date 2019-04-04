using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBox
{
    public static class Extensions
    {
        public static IEnumerable<Argument> Find(this IEnumerable<Argument> args, string name)
        {
            return from f in args
                   where f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                   select f;
        }

        public static IEnumerable<Argument> Find(this IEnumerable<Argument> args, string[] names)
        {
            return from f in args
                   where names.Contains(f.Name, StringComparer.InvariantCultureIgnoreCase)
                   select f;
        }

        public static int Count(this IEnumerable<Argument> args, string name)
        {
            return Find(args, name).Count();
        }

        public static int Count(this IEnumerable<Argument> args, string[] names)
        {
            return Find(args, names).Count();
        }

        public static bool HasArgument(this IEnumerable<Argument> args, string name)
        {
            return (Count(args, name) >= 1);
        }

        public static string GetValue(this IEnumerable<Argument> args, params string[] name)
        {
            string result = null;
            foreach (var item in name)
            {
                Argument arg = args.Find(name).SingleOrDefault();
                if (!arg.IsEmpty)
                {
                    result = arg.Value;
                    break;
                }
            }

            return result;
        }

        public static T GetValue<T>(this IEnumerable<Argument> args, params string[] name)
        {
            T result = default(T);
            try
            {
                // forward the call to retrieve the parameter value
                string raw = GetValue(args, name);
                // attempt to convert the value
                result = (T)ConversionUtility.ChangeType(raw, typeof(T));
            }
            catch
            {
            }

            return result;
        }

        public static string Serialize(this IEnumerable<Argument> args)
        {
            bool isFirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (var arg in args)
            {
                if (isFirst)
                {
                    sb.Append(arg.Value);
                    isFirst = false;
                }
                else
                {
                    sb.AppendFormat(" -{0} {1}", arg.Name, arg.Value);
                }
            }

            return sb.ToString();
        }

        public static void Bind(this IEnumerable<Argument> args, object model)
        {
            Bind(args, KnownScopeName.Default, model);
        }

        public static void Bind(this IEnumerable<Argument> args, string scopeName, object model)
        {
            var properties = model.GetType().GetProperties();
            foreach (var property in properties)
            {
                ArgumentBindingAttribute attrib = (from f in ReflectionUtility.GetAttributesOfType<ArgumentBindingAttribute>(property)
                                                   where scopeName.Equals(f.ScopeName, StringComparison.InvariantCultureIgnoreCase)
                                                   select f).SingleOrDefault();
                if (null == attrib)
                {
                    attrib = new ArgumentBindingAttribute(scopeName, new string[] { property.Name });
                }

                string value = args.GetValue(attrib.ArgumentNames);

                object raw = (null != value) ? ConversionUtility.ChangeType(value, property.PropertyType) : value;

                if (null != property.SetMethod)
                {
                    property.SetValue(model, raw);
                }
            }
        }
    }
}
