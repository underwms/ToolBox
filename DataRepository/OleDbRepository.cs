using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace DataTemplate
{
    public class OleDbRepository : IOleDbRepository
    {
        public async Task<DataTable> GetSpreadsheetDataAsync(string path)
        {
            var sheetData = new DataTable();
            var connectionString = DataConfig.OleDBSpreadsheetConnection(path);

            using (var conn = new OleDbConnection(connectionString))
            {
                await conn.OpenAsync();
                var sheetAdapter = new OleDbDataAdapter("select * from [Sheet1$]", conn);
                sheetAdapter.Fill(sheetData);
            }

            return sheetData;
        }
    }
}
