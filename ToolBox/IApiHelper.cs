using System.Threading.Tasks;

namespace ToolBox
{
    public interface IApiHelper
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsync<T>(string url, object dataPackage);
    }
}