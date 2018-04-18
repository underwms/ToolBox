using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToolBox;

namespace Azure
{
    public class AzureTableStorageManager
    {
        private readonly CloudTableClient _tableClient;
        private readonly string _connectionString;
        private readonly string _tableName;

        
        public AzureTableStorageManager(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;

            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        
        public static string RowKeyFromType(string rowType)
        {
            var retVal = rowType;
            var index = retVal.LastIndexOf(".", StringComparison.Ordinal);

            if (index > 0)
            { retVal = retVal.Substring(index + 1); }

            return retVal;
        }

        public async Task<T> GetFullObjectAsync<T>(long id)
            where T : AzureTableEntityBase
        {
            var retVal = default(T);

            try
            {
                var cloudTable = _tableClient.GetTableReference(_tableName);
                var retrieveOperation = TableOperation.Retrieve<AzureTableJsonEntity<T>>(
                    id.ToString(),
                    RowKeyFromType(typeof(T).ToString()));
                var tableResult = await cloudTable.ExecuteAsync(retrieveOperation);
                
                if (!(tableResult is null))
                {
                    var tableEntity = (AzureTableJsonEntity<T>) tableResult.Result;

                    JsonHelper.TryParse(tableEntity.JsonData, out retVal);
                    retVal.ETag = tableEntity.ETag;
                    retVal.Timestamp = tableEntity.Timestamp;
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }
    }
}
