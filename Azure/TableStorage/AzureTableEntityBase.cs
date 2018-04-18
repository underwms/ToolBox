using System;

namespace Azure
{
    public abstract class AzureTableEntityBase
    {
        public string ETag { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
