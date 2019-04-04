namespace ServiceModel
{
    public interface IService<T, R> : IService
        where T : ServiceModelBase
        where R : ServiceResult, new()
    {
        R Process(T model);
    }

    /// <summary>
    /// Marker interface for an IService implementing type.
    /// </summary>
    /// <remarks>
    /// The implementation is empty.
    /// </remarks>
    public interface IService
    {
        /// <summary>
        /// Event that is raised by the service when an informational message is generated during processing.
        /// </summary>
        event ServiceEventHandler OnInfo;

        /// <summary>
        /// Event that is raised by the service when a warning is generated during processing.
        /// </summary>
        event ServiceEventHandler OnWarn;

        /// <summary>
        /// Event that is raised by the service when an error has occurred during processing.
        /// </summary>
        event ServiceEventHandler OnError;
    }
}
