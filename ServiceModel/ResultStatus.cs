namespace ServiceModel
{
    /// <summary>
    /// Enumeration of result status values.
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// The result status is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The operation succeeded.
        /// </summary>
        Success,
        /// <summary>
        /// The operation partially succeeded.
        /// </summary>
        Partial,
        /// <summary>
        /// The operation failed.
        /// </summary>
        Failed,
        /// <summary>
        /// The operation resulted in an exception.
        /// </summary>
        Exception
    }
}
