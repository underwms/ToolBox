using System.Data;
using System.Threading.Tasks;

namespace DataTemplate
{
    public interface IOleDbRepository
    {
        Task<DataTable> GetSpreadsheetDataAsync(string path);
    }
}
