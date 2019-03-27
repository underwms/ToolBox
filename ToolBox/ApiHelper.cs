using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ToolBox
{
    public class ApiHelper : IApiHelper
    {
        // Notes ----------------------------------------------------------------------------------
        // the following sites are the basis for this wrapper class
        // https://johnthiriet.com/efficient-api-calls/
        // https://johnthiriet.com/efficient-post-calls/

        // Private fields -------------------------------------------------------------------------
        private readonly HttpClientHandler _credentials = new HttpClientHandler { Credentials = CredentialCache.DefaultCredentials };
        private readonly TimeSpan _timeOut = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
        private readonly HttpClient _httpClient;

        // Constructors ---------------------------------------------------------------------------
        public ApiHelper() =>
            _httpClient = new HttpClient(_credentials) { Timeout = _timeOut };

        public ApiHelper(HttpMessageHandler clientHandler) =>
            _httpClient = new HttpClient(clientHandler) { Timeout = _timeOut };
        
        public ApiHelper(TimeSpan timeout) =>
            _httpClient = new HttpClient(_credentials) { Timeout = timeout };

        public ApiHelper(HttpMessageHandler clientHandler, TimeSpan timeout) =>
            _httpClient = new HttpClient(clientHandler) {Timeout = timeout};

        // Public methods -------------------------------------------------------------------------
        public async Task<T> GetAsync<T>(string url)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            { return await HandleHttpResponse<T>(response); }
        }

        public async Task<T> PostAsync<T>(string url, object dataPackage)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateJsonHttpContent(dataPackage))
            {
                request.Content = httpContent;

                using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                { return await HandleHttpResponse<T>(response); }
            }
        }

        // Private helper methods -----------------------------------------------------------------
        private static async Task<T> HandleHttpResponse<T>(HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync();
                
            if (response.IsSuccessStatusCode)
            { return JsonHelper.DeserializeJsonFromStream<T>(stream); }
                
            await ThrowApiException(response.StatusCode, stream);
            return default(T);  //this is only to appease the syntax gods; this will never actually be hit
        }

        private static async Task ThrowApiException(HttpStatusCode responseStatusCode, Stream stream)
        {
            var content = await StreamToStringAsync(stream);
            throw new ApiException
            {
                StatusCode = (int)responseStatusCode,
                Content = content
            };
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            if (stream == null) 
            { return null; }
            
            string content;
            using (var sr = new StreamReader(stream))
            { content = await sr.ReadToEndAsync(); }

            return content;
        }

        private static HttpContent CreateJsonHttpContent(object content)
        {
            if (content == null) 
            { return null; }

            var ms = new MemoryStream();
            JsonHelper.SerializeJsonIntoStream(content, ms);
            ms.Seek(0, SeekOrigin.Begin);

            HttpContent httpContent = new StreamContent(ms);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpContent;
        }
    }

    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }
}
