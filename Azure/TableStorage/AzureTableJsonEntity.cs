using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure
{
    public class AzureTableJsonEntity<T> : TableEntity
    {
        public string JsonData { get; set; }

        public AzureTableJsonEntity() { }

        public AzureTableJsonEntity(T t, string partitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = AzureTableStorageManager.RowKeyFromType(t.GetType().ToString());
        }
    }
}
