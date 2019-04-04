using System;
using System.Configuration;
using ToolBox;

namespace ServiceModel
{
    /// <summary>
    /// Abstract.  Base class for other services that implements the <see cref="IService{T}"/> interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Service<T, R> : IService<T, R> 
        where T : ServiceModelBase
        where R : ServiceResult, new()
    {
        /// <summary>
        /// Performs processing logic for this instance using the specified model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public R Process(T model)
        {
            R result = new R();

            OnProcess(model, result);

            return result;
        }

        /// <summary>
        /// Abstract.  When overridden in a derived type, provides the implementation for the 
        /// processing logic performed by this instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        protected abstract void OnProcess(T model, R result);

        /// <summary>
        /// Event that is raised by the service when an informational message is generated during processing.
        /// </summary>
        public event ServiceEventHandler OnInfo;

        /// <summary>
        /// Event that is raised by the service when a warning is generated during processing.
        /// </summary>
        public event ServiceEventHandler OnWarn;

        /// <summary>
        /// Event that is raised by the service when an error has occurred during processing.
        /// </summary>
        public event ServiceEventHandler OnError;

        /// <summary>
        /// Raises the <see cref="OnInfo"/> event.
        /// </summary>
        /// <param name="message"></param>
        protected void RaiseInfo(string message)
        {
            OnInfo?.Invoke(this, new ServiceEventArgs(ServiceEventType.Information, message));
        }

        /// <summary>
        /// Raises the <see cref="OnInfo"/> event.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        protected void RaiseInfo(string format, params object[] args)
        {
            OnInfo?.Invoke(this, new ServiceEventArgs(ServiceEventType.Information, String.Format(format, args)));
        }

        /// <summary>
        /// Raises the <see cref="OnWarn"/> event.
        /// </summary>
        /// <param name="message"></param>
        protected void RaiseWarning(string message)
        {
            OnWarn?.Invoke(this, new ServiceEventArgs(ServiceEventType.Warning, message));
        }

        /// <summary>
        /// Overloaded.  Raises the <see cref="OnError"/> event.
        /// </summary>
        /// <param name="ex"></param>
        protected void RaiseError(Exception ex)
        {
            OnError?.Invoke(this, new ServiceEventArgs(ServiceEventType.Error, ex.Message, ex));
        }

        /// <summary>
        /// Overloaded.  Raises the <see cref="OnError"/> event.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        protected void RaiseError(string message, Exception ex)
        {
            OnError?.Invoke(this, new ServiceEventArgs(ServiceEventType.Error, message, ex));
        }

        /// <summary>
        /// Helper method that checks to make sure a required condition is not satisfied before throwing
        /// a <see cref="SystemException"/> using the specified message.
        /// </summary>
        /// <param name="condition">The boolean condition to evaluate.</param>
        /// <param name="message">The exception message text.</param>
        protected void Enforce(bool condition, string message)
        {
            if (condition)
            {
                throw new SystemException(message);
            }
        }

        /// <summary>
        /// Retrieves a named value from the configuration file for the current
        /// application domain.
        /// </summary>
        /// <typeparam name="ReturnType"></typeparam>
        /// <param name="key">The key name to retrieve.</param>
        /// <returns></returns>
        protected static ReturnType GetConfigSetting<ReturnType>(string key)
        {
            return GetConfigSetting<ReturnType>(key, default(ReturnType));
        }

        /// <summary>
        /// Retrieves a named value from the configuration file for the current
        /// application domain using the specified default value when undefined.
        /// </summary>
        /// <typeparam name="ReturnType"></typeparam>
        /// <param name="key">The key name to retrieve.</param>
        /// <param name="defaultValue">The default value to use when the value is not defined.</param>
        /// <returns></returns>
        protected static ReturnType GetConfigSetting<ReturnType>(string key, ReturnType defaultValue)
        {
            ReturnType result = default(ReturnType);
            
            try
            {
                string raw = ConfigurationManager.AppSettings[key];

                if (!String.IsNullOrEmpty(raw))
                {
                    result = (ReturnType)ConversionUtility.ChangeType(raw, typeof(ReturnType));
                }
                else
                {
                    result = defaultValue;
                }
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
