using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataRepository
{
    public interface IDapperUtils
    {
        Task StoredProcedureNonQueryWithParametersAsync(string proc, Dictionary<string, object> args = null);
        Task<T> StoredProcedureScalarWithParametersAsync<T>(string proc, Dictionary<string, object> args = null);
        Task<IEnumerable<T>> StoredProcedureQueryWithParametersAsync<T>(string proc, Dictionary<string, object> args = null);
        Task StoredProcedureNonQueryWithXmlAsync(string proc, string xml);
        Task<T> StoredProcedureScalarWithXmlAsync<T>(string proc, string xml);
        Task<IEnumerable<T>> StoredProcedureQueryWithXmlAsync<T>(string proc, string xml);
        Task SqlNonQueryWithParametersAsync(string sql, Dictionary<string, object> args = null);
        Task<T> SqlScalarWithParametersAsync<T>(string sql, Dictionary<string, object> args = null);
        Task<IEnumerable<T>> SqlQueryWithParametersAsync<T>(string sql, Dictionary<string, object> args = null);
        Task SqlNonQueryWithXmlAsync(string sql, string xml);
        Task<T> SqlScalarWithXmlAsync<T>(string sql, string xml);
        Task<IEnumerable<T>> SqlQueryWithXmlAsync<T>(string sql, string xml);
    }
}