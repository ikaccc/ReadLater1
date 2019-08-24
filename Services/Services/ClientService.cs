using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReadLater.Services
{
    public class ClientService : IClientService
    {
        public HttpClient Client { get; set; }

        public ClientService()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"])
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //should be some token
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", EncodeTo64("ApiUserName:ApiPassword"));
            Client.DefaultRequestHeaders.Add("X-ApiKey", "RandomApiKeyValue");
        }

        public static string EncodeTo64(string toEncode)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public async Task<HttpResponseMessage> GetResponse(string url)
        {
            try
            {
                return await Client.GetAsync(url);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<HttpResponseMessage> GetMostPopularBookmarks()
        {
            try
            {
                return await Client.GetAsync("bookmarks/mostpopularbookmarks");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<HttpResponseMessage> GetResponse(int id, string url)
        {
            try
            {
                return await Client.GetAsync(url + id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<HttpResponseMessage> PutResponse(object model, string url)
        {
            var jsonString = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            try
            {
                return await Client.PutAsync(url, content);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<HttpResponseMessage> PostResponse(object model, string url)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                return await Client.PostAsync(url, content);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<HttpResponseMessage> DeleteResponse(int id, string url)
        {
            try
            {
                return await Client.DeleteAsync(url + id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
