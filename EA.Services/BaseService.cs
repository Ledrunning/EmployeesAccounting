using RestSharp;
using Newtonsoft.Json;

namespace EA.Services
{
    public class BaseService
    {
        protected readonly string BaseUrl;
        private readonly int timeout;

        public BaseService(string baseUrl, int timeout)
        {
            BaseUrl = baseUrl;
            this.timeout = timeout;
        }

        protected T GetContent<T>(RestResponseBase response, string url)
        {
            if (response.IsSuccessful)
            {
                if (response.Content != null)
                {
                    var model = JsonConvert.DeserializeObject<T>(response.Content);
                    if (model != null)
                    {
                        return model;
                    }
                }
            }

            throw new ApplicationException(
                $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected RestClientOptions SetOptions(Uri url)
        {
            return new RestClientOptions(url)
            {
                ThrowOnAnyError = true,
                MaxTimeout = timeout
            };
        }
    }
}