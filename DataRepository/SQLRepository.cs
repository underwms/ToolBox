using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTemplate
{
    public class SQLRepository : ISQLRepository
    {
        private static async Task<IEnumerable<T>> GetList<T>(string procName, Dictionary<string, object> parameters = null)
        {
            var retVal = new List<T>();

            try
            {
                using (var sqlConnection = new SqlConnection(DataConfig.SqlDefenderConnection))
                {
                    var queryParams = new DynamicParameters();
                    if (!(parameters is null))
                    {
                        foreach (var param in parameters)
                        { queryParams.Add($"@{param.Key}", param.Value); }
                    }

                    var dbResult = await sqlConnection.QueryAsync<T>(
                        procName,
                        queryParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 180);

                    retVal = dbResult.ToList();
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }

        private static async Task<T> ScalarXML<T>(string procName, string xml)
        {
            var id = (T)Activator.CreateInstance(typeof(T));

            try
            {
                using (var sqlConnection = new SqlConnection(DataConfig.SqlDefenderConnection))
                {
                    var queryParams = new DynamicParameters();
                    queryParams.Add($"@XML", xml);

                    var dbResult = await sqlConnection.ExecuteScalarAsync<T>(
                        sql: procName,
                        param: queryParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 180);

                    id = dbResult;
                }
            }
            catch (Exception ex)
            { throw; }

            return id;
        }

        private static async Task ExecuteXML(string procName, string xml)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(DataConfig.SqlDefenderConnection))
                {
                    var queryParams = new DynamicParameters();
                    queryParams.Add($"@XML", xml);

                    var dbResult = await sqlConnection.ExecuteAsync(
                        sql: procName,
                        param: queryParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 180);
                }
            }
            catch (Exception ex)
            { throw; }
        }
    }
}
