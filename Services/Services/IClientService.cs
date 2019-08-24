using System.Net.Http;
using System.Threading.Tasks;

namespace ReadLater.Services
{
    public interface IClientService
    {
        Task<HttpResponseMessage> GetResponse(string url);
        //Task<IEnumerable<T>> GetResponse<T>(string url);
        Task<HttpResponseMessage> GetResponse(int id, string url);
        Task<HttpResponseMessage> PutResponse(object model, string url);
        Task<HttpResponseMessage> PostResponse(object model, string url);
        Task<HttpResponseMessage> DeleteResponse(int id, string url);
        Task<HttpResponseMessage> GetMostPopularBookmarks();
    }
}
