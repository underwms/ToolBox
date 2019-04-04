namespace ServiceModel
{
    /// <summary>
    /// The result type for a service operation.
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Gets or sets the result status.
        /// </summary>
        public ResultStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag object for the result.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message for the result.
        /// </summary>
        public string Message
        {
            get;
            set;
        }
    }
}
