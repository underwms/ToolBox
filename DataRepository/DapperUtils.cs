using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataRepository
{
    public class DapperUtils : IDapperUtils
    {
        // Stored Procedure Processing ------------------------------------------------------------
        public async Task StoredProcedureNonQueryWithParametersAsync(string proc, Dictionary<string, object> args = null)
        {
            try
            {
                var connectionString = GetConnectString("CustomDb");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var queryParams = BuildDynamicParameters(args);

                    var dbResult = await sqlConnection.ExecuteAsync(
                        sql: proc,
                        param: queryParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 600);
                }
            }
            catch (Exception ex)
            { throw; }
        }
        
        public async Task<T> StoredProcedureScalarWithParametersAsync<T>(string proc, Dictionary<string, object> args = null)
        {
            var retVal = (T)Activator.CreateInstance(typeof(T));

            try
            {
                var connectionString = GetConnectString("CustomDb");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var queryParams = BuildDynamicParameters(args);

                    var dbResult = await sqlConnection.ExecuteScalarAsync<T>(
                        sql: proc,
                        param: queryParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 600);

                    retVal = dbResult;
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }
        
        public async Task<IEnumerable<T>> StoredProcedureQueryWithParametersAsync<T>(string proc, Dictionary<string, object> args = null)
        {
            var retVal = new List<T>();

            try
            {
                var connectionString = GetConnectString("CustomDb");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var queryParams = BuildDynamicParameters(args);

                    var dbResult = await sqlConnection.QueryAsync<T>(
                        sql: proc,
                        param: queryParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 600);
                    
                    retVal = dbResult.ToList();
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }

        public async Task StoredProcedureNonQueryWithXmlAsync(string proc, string xml) =>
            await StoredProcedureNonQueryWithParametersAsync(proc, new Dictionary<string, object>{["XML"] = xml});
 
        public async Task<T> StoredProcedureScalarWithXmlAsync<T>(string proc, string xml) =>
            await StoredProcedureScalarWithParametersAsync<T>(proc, new Dictionary<string, object>{["XML"] = xml});
        
        public async Task<IEnumerable<T>> StoredProcedureQueryWithXmlAsync<T>(string proc, string xml) =>
            await StoredProcedureQueryWithParametersAsync<T>(proc, new Dictionary<string, object>{["XML"] = xml});

        // SQL Processing -------------------------------------------------------------------------
        public async Task SqlNonQueryWithParametersAsync(string sql, Dictionary<string, object> args = null)
        {
            try
            {
                var connectionString = GetConnectString("CustomDb");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var queryParams = BuildDynamicParameters(args);

                    var dbResult = await sqlConnection.ExecuteAsync(
                        sql: sql,
                        param: queryParams,
                        commandTimeout: 600);
                }
            }
            catch (Exception ex)
            { throw; }
        }

        public async Task<T> SqlScalarWithParametersAsync<T>(string sql, Dictionary<string, object> args = null)
        {
            var retVal = (T)Activator.CreateInstance(typeof(T));

            try
            {
                var connectionString = GetConnectString("CustomDb");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var queryParams = BuildDynamicParameters(args);

                    var dbResult = await sqlConnection.ExecuteScalarAsync<T>(
                        sql: sql,
                        param: queryParams,
                        commandTimeout: 600);

                    retVal = dbResult;
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }
        
        public async Task<IEnumerable<T>> SqlQueryWithParametersAsync<T>(string sql, Dictionary<string, object> args = null)
        {
            var retVal = new List<T>();

            try
            {
                var connectionString = GetConnectString("CustomDb");
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var queryParams = BuildDynamicParameters(args);

                    var dbResult = await sqlConnection.QueryAsync<T>(
                        sql: sql,
                        param: queryParams,
                        commandTimeout: 600);
                    
                    retVal = dbResult.ToList();
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }
        
        public async Task SqlNonQueryWithXmlAsync(string sql, string xml) =>
            await SqlNonQueryWithParametersAsync(sql, new Dictionary<string, object>{["XML"] = xml});
        
        public async Task<T> SqlScalarWithXmlAsync<T>(string sql, string xml) =>
            await SqlScalarWithParametersAsync<T>(sql, new Dictionary<string, object>{["XML"] = xml});
        
        public async Task<IEnumerable<T>> SqlQueryWithXmlAsync<T>(string sql, string xml) =>
            await SqlQueryWithParametersAsync<T>(sql, new Dictionary<string, object>{["XML"] = xml});
        
        // Private Methods ------------------------------------------------------------------------
        private static string GetConnectString(string dbName) =>
            ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        
        private static DynamicParameters BuildDynamicParameters(Dictionary<string, object> args)
        {
            var queryParams = new DynamicParameters();
            if (args is null) return queryParams;

            foreach (var param in args)
            { queryParams.Add($"@{param.Key}", param.Value); }

            return queryParams;
        }
    }
}
