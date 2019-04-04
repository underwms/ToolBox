using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ToolBox;
using WebApiExample.Models;
using WebApiExample.NewFolder;

namespace WebApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [HttpPost]
        public TestClass Post([FromBody] TestClass myClass)
        {
            return new TestClass() { Message = "RECEIVED: " + myClass.Message };
        }

        // GET api/values
        [HttpGet]
        [Route("InvokeProcess")]
        public ActionResult<object> InvokeProcess(string serviceType)
        {
            var args = HttpContext.Request.Query
                .Select(kvp => new Argument(kvp.Key, kvp.Value));

            var result = new OperationResult<InvokeResult>();
            result.Data.Trace = string.Format("You invoked: {0}", serviceType);

            if (!string.IsNullOrEmpty(serviceType))
            {
                // EXAMPLE: invoke -service:HPTS.Framework.Services.Utility.SQL.ColumnChangeScriptService -s:Foo -t:Bar -c:Quux -dt:Int
                Type type = TypeUtility.Find(serviceType, assemblyPath: null);
                if (null != type)
                {
                    IService service = Activator.CreateInstance(type) as IService;
                    if (null != service)
                    {
                        //// wire up events
                        //service.OnInfo += Service_OnNotify;
                        //service.OnWarn += Service_OnNotify;
                        //service.OnError += Service_OnNotify;

                        // we know this is an instance of our service type because of the marker interface
                        MethodInfo method = ReflectionUtility.GetMethod(service, "Process");
                        if (null != method)
                        {
                            var mod = method.GetParameters().FirstOrDefault();
                            if (null != mod)
                            {
                                Type modelType = mod.ParameterType;
                                if ((null != modelType) && (typeof(ServiceModelBase).IsAssignableFrom(modelType)))
                                {
                                    ServiceModelBase modelBase = Activator.CreateInstance(modelType) as ServiceModelBase;
                                    if (null != modelBase)
                                    {
                                        args.Bind(modelBase);
                                        ServiceResult serviceResult = method.Invoke(service, new object[] { modelBase }) as ServiceResult;
                                        if (null != serviceResult)
                                        {
                                            WriteLine("Result: {0}\r\n", serviceResult.Status);
                                            WriteLine(serviceResult.ToString());
                                            result.Data.Result = serviceResult;
                                        }
                                        else
                                        {
                                            WriteLine("The service was invoked but no result was returned.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    WriteLine("The specified service type could not be located.");
                }

            }

            return result;
        }

        /// <summary>
        /// Writes a formatted string to the trace log.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        protected void WriteFormat(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }

        /// <summary>
        /// Writes an empty line to the trace log.
        /// </summary>
        protected void WriteLine()
        {
            WriteLine(String.Empty);
        }

        /// <summary>
        /// Writes a line of text to the trace log.
        /// </summary>
        /// <param name="text"></param>
        protected void WriteLine(string text)
        {
            // TODO: add implementation
        }

        /// <summary>
        /// Writes a formatted line of text to the trace log.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        protected void WriteLine(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }
    }
}
