using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ToolBox
{
    /// <summary>
    /// Utility class that performs operations related to <see cref="Type"/> instances.
    /// </summary>
    public static class TypeUtility
    {
        /// <summary>
        /// Returns a list of <see cref="Type"/> instances for all derived, non-abstract types
        /// for the specified base type within the current list of assemblies for the current application domain.
        /// </summary>
        /// <typeparam name="BaseType">The base type for which to search for derived types.</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindDerivedTypes<BaseType>()
        {
            // declare result list
            List<Type> list = new List<Type>();

            // get all assemblies in the current application domain
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // iterate each assemly
            foreach (var assembly in assemblies)
            {
                try
                {
                    // get all types in all assemblies in the current application domain
                    var types = assembly.GetTypes();

                    // iterate each type found above
                    foreach (var type in types)
                    {
                        // check to see if this type can be assigned from the base type
                        if ((typeof(BaseType).IsAssignableFrom(type))
                            && (!type.IsAbstract))
                        {
                            // add the matching item to the list
                            list.Add(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // enable this block of code to determine why assembly type loading is failing
#if DEBUG
                    //if (ex is ReflectionTypeLoadException)
                    //{
                    //    ReflectionTypeLoadException rtle = (ReflectionTypeLoadException)ex;
                    //    if (null != rtle.LoaderExceptions)
                    //    {
                    //        var loaderExceptions = rtle.LoaderExceptions;
                    //        StringBuilder sb = new StringBuilder();
                    //        foreach (var loaderException in loaderExceptions)
                    //        {
                    //            sb.Append(loaderException.ToString());
                    //        }
                    //        throw new CoreException(sb.ToString());
                    //    }
                    //}
#endif
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a listing of <see cref="Type"/>s that are decorated with the specified
        /// <see cref="Attribute"/> type.
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindTypesWithAttribute<AttributeType>() where AttributeType : Attribute
        {
            // get all types in all assemblies in the current application domain
            var types = (from f in AppDomain.CurrentDomain.GetAssemblies()
                         select f.GetTypes()).SelectMany(f => f);

            // iterate each type
            foreach (var type in types)
            {
                // check to see if this type has one or more attributes
                var attribs = ReflectionUtility.GetAttributesOfType<AttributeType>(type);
                if ((null != attribs) && (attribs.Count() > 0))
                {
                    // return this matching type
                    yield return type;
                }
            }

            yield break;
        }

        public static Type Find(string[] formats, params string[] args)
        {
            Type result = null;

            foreach (string format in formats)
            {
                string typeName = String.Format(format, args);
                try
                {
                    result = Type.GetType(typeName);
                }
                catch
                {
                }

                if (null != result) break;
            }

            return result;
        }

        /// <summary>
        /// Finds a <see cref="Type"/> by the specified type name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type Find(string typeName)
        {
            Type type = Type.GetType(typeName);

            #region Look in well-known assemblies
            if ((null == type) && (!typeName.Contains(StringHelper.Comma)))
            {
                type = Find(new string[]
                {
                    // TODO: define these as constants?
                    "{0}, HPTS.Framework.Core",
                    "{0}, HPTS.Framework.Sys"
                },
                typeName);
            }
            #endregion

            #region Iterate all loaded assemblies in the application domain
            if (null == type)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    try
                    {
                        type = assembly.GetType(typeName);
                    }
                    catch
                    {
                    }
                    if (null != type) break;
                }
            }
            #endregion

            #region Load all assemblies in the current execution path location
            if (null == type)
            {
                try
                {
                    string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string[] files = System.IO.Directory.GetFiles(path, "*.dll");
                    foreach (string file in files)
                    {
                        try
                        {
                            Assembly assembly = Assembly.LoadFile(file);
                            if (null != assembly)
                            {
                                type = assembly.GetType(typeName);
                                if (null != type) break;
                            }
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
            #endregion

            return type;
        }

        public static Type Find(string typeName, string assemblyPath)
        {
            Type type = null;

            if (!String.IsNullOrEmpty(assemblyPath))
            {
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                if (null != assembly)
                {
                    type = assembly.GetType(typeName);

                    // make sure the type was found
                    if (null != type)
                    {
                        // load any other assemblies from this path location
                        LoadAssembliesFromPath(System.IO.Path.GetDirectoryName(assemblyPath));
                    }
                }
            }

            // check to see if the type was loaded
            if (null == type)
            {
                // EXAMPLE: invoke -service:HPTS.Framework.Services.Utility.SQL.ColumnChangeScriptService -s:Foo -t:Bar -c:Quux -dt:Int
                type = TypeUtility.Find(typeName);
            }

            return type;
        }

        public static void LoadAssembliesFromPath(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                try
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        string[] files = System.IO.Directory.GetFiles(path, "*.dll");
                        foreach (string file in files)
                        {
                            try
                            {
                                Assembly assembly = Assembly.LoadFrom(file);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
