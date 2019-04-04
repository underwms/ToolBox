using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ToolBox
{
    /// <summary>
    /// Utility class that defines reflection-based operations.
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// Retrieves a list of attributes of the specified type (when applicable) for the specified member.
        /// </summary>
        /// <typeparam name="AttributeType">The <see cref="Attribute"/> derived type.</typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static IEnumerable<AttributeType> GetAttributesOfType<AttributeType>(MemberInfo memberInfo) where AttributeType : Attribute
        {
            IEnumerable<AttributeType> attributes = null;

            Type attributeType = typeof(AttributeType);

            if (null != memberInfo)
            {
                attributes = memberInfo.GetCustomAttributes(attributeType, false).OfType<AttributeType>();
            }

            return attributes;
        }

        /// <summary>
        /// Retrieves a list of attributes of the specified type (when applicable) for the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<AttributeType> GetAttributesOfType<AttributeType>(Type type) where AttributeType : Attribute
        {
            IEnumerable<AttributeType> attributes = null;

            Type attributeType = typeof(AttributeType);

            if (null != type)
            {
                attributes = type.GetCustomAttributes(attributeType, false).OfType<AttributeType>();
            }

            return attributes;
        }

        /// <summary>
        /// Retrieves a single attribute definition of the specified type (if it exists) for the specified source object.
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static AttributeType GetSingleAttribute<AttributeType>(object source) where AttributeType : Attribute
        {
            return GetSingleAttribute<AttributeType>(source.GetType());
        }

        /// <summary>
        /// Retrieves a single attribute definition of the specified type (if it exists) for the specified member.
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static AttributeType GetSingleAttribute<AttributeType>(MemberInfo memberInfo) where AttributeType : Attribute
        {
            AttributeType attribute = null;

            Type attributeType = typeof(AttributeType);

            if (null != memberInfo)
            {
                attribute = memberInfo.GetCustomAttributes(attributeType, false).OfType<AttributeType>().SingleOrDefault();
            }

            return attribute;
        }

        /// <summary>
        /// Retrieves a single attribute definition of the specified type (if it exists) for the specified parameter.
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public static AttributeType GetSingleAttribute<AttributeType>(ParameterInfo parameterInfo) where AttributeType : Attribute
        {
            AttributeType attribute = null;

            Type attributeType = typeof(AttributeType);

            if (null != parameterInfo)
            {
                attribute = parameterInfo.GetCustomAttributes(attributeType, false).OfType<AttributeType>().SingleOrDefault();
            }

            return attribute;
        }

        /// <summary>
        /// Retrieves a single attribute of the specified type for the supplied enumeration value.
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AttributeType GetSingleAttribute<AttributeType>(Enum value) where AttributeType : Attribute
        {
            AttributeType attribute = null;

            Type attributeType = typeof(AttributeType);

            if (null != value)
            {
                attribute = value.GetType().GetField(value.ToString()).GetCustomAttributes(attributeType, false).OfType<AttributeType>().SingleOrDefault();
            }

            return attribute;
        }

        /// <summary>
        /// Returns a description for the specified member (when available via a <see cref="DescriptionAttribute"/>).
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetDescription(MemberInfo memberInfo)
        {
            string result = null;
            var attrib = ReflectionUtility.GetSingleAttribute<DescriptionAttribute>(memberInfo);
            if ((null != attrib) && (!String.IsNullOrEmpty(attrib.Description)))
            {
                result = attrib.Description;
            }
            return result;
        }

        /// <summary>
        /// Returns a description for the specified value (when available via a <see cref="DescriptionAttribute"/>).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
        {
            string result = null;
            var attrib = ReflectionUtility.GetSingleAttribute<DescriptionAttribute>(value);
            if ((null != attrib) && (!String.IsNullOrEmpty(attrib.Description)))
            {
                result = attrib.Description;
            }
            return result;
        }

        /// <summary>
        /// Retrieves the value of the specified named property on the supplied instance
        /// (when it exists).
        /// </summary>
        /// <typeparam name="T">The type as which to return the property value.</typeparam>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <param name="instance">The instance from which to retrieve the property value.</param>
        /// <returns></returns>
        public static bool TryGetPropertyValue<T>(string propertyName, object instance, out T result)
        {
            bool successful = false;
            result = default(T);

            try
            {
                if (null != instance)
                {
                    Type type = instance.GetType();
                    if (!String.IsNullOrEmpty(propertyName))
                    {
                        PropertyInfo propertyInfo = type.GetProperty(propertyName);
                        if (null != propertyInfo)
                        {
                            result = (T)propertyInfo.GetValue(instance, null);
                            successful = true;
                        }
                    }
                }
            }
            catch
            {
            }

            return successful;
        }

        /// <summary>
        /// Returns the value of the specified property name for the supplied instance
        /// (or null if the property does not exist).
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object instance, string propertyName)
        {
            object result = null;

            // verify the instance
            if (null != instance)
            {
                // look for the property on the type
                PropertyInfo property = instance.GetType().GetProperty(propertyName);
                if (null != property)
                {
                    // retrieve the value
                    result = property.GetValue(instance, null);
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves the method having the specified name on the given object
        /// instances (if it exists).
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        /// <returns>The method instance or null.</returns>
        public static MethodInfo GetMethod(object instance, string methodName)
        {
            MethodInfo method = null;

            if (null != instance)
            {
                method = instance.GetType().GetMethod(methodName);
            }

            return method;
        }

        public static bool HasProperty(object instance, string propertyName)
        {
            bool result = false;

            if (null != instance)
            {
                PropertyInfo property = instance.GetType().GetProperty(propertyName);
                result = (null != property);
            }

            return result;
        }

        public static bool SetPropertyValue(object instance, string propertyName, object value)
        {
            bool result = false;

            if (null != instance)
            {
                try
                {
                    PropertyInfo property = instance.GetType().GetProperty(propertyName);
                    if (null != property)
                    {
                        property.SetValue(instance, value, null);
                        result = true;
                    }
                }
                catch
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the parameters for the specified type name and method name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static ParameterInfo[] GetMethodParameters(string typeName, string methodName)
        {
            ParameterInfo[] methodParameters = null;

            if (!String.IsNullOrEmpty(typeName))
            {
                // get the type
                Type type = Type.GetType(typeName);

                if (null != type)
                {
                    // get the method info 
                    MethodInfo methodInfo = type.GetMethod(methodName);

                    if (null != methodInfo)
                    {
                        // get the parameters for the method
                        methodParameters = methodInfo.GetParameters();
                    }
                }
            }

            return methodParameters;
        }

        /// <summary>
        /// Copies all properties from the source instance to the target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool CopyProperties(object source, object target)
        {
            if ((null == source) || (null == target)) return false;

            if (!source.GetType().Equals(target.GetType())) return false;

            bool result = true;

            PropertyInfo[] properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    property.SetValue(target, property.GetValue(source));
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
