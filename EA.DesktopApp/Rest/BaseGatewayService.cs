using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Exceptions;
using Newtonsoft.Json;
using RestSharp;

namespace EA.DesktopApp.Rest
{
    // TODO add main login - password into basic auth here
    public class BaseGatewayService
    {
        private readonly int _timeout;
        protected readonly string BaseUrl;
        protected readonly string PingUrl;
        protected readonly int MaxPingAttempts;
        protected readonly int ServerPingTimeout;

        public BaseGatewayService(AppConfig appConfig)
        {
            BaseUrl = appConfig.BaseServerUri;
            _timeout = appConfig.Timeout;
            PingUrl = appConfig.ServerPingUri;
            MaxPingAttempts = appConfig.MaxPingAttempts;
            ServerPingTimeout = appConfig.ServerPingTimeout;
        }

        protected T GetContent<T>(RestResponseBase response)
        {
            CheckResponse(response);

            var model = JsonConvert.DeserializeObject<T>(response.Content);
            if (model != null)
            {
                return model;
            }

            throw new ApiException(
                $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected void CheckResponse(RestResponseBase response)
        {
            if (!response.IsSuccessful)
            {
                throw new ApiException(
                    $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
            }

            if (response.Content == null)
            {
                throw new ApiException(
                    $"Response from service is failed. Status code: {response.StatusCode}, {response.ErrorMessage}");
            }
        }

        protected async Task<RestResponse> SendRequestAsync(Uri url, Method method, CancellationToken cancellationToken)
        {
            var client = new RestClient(SetOptions(url));
            var request = new RestRequest(url, method);
            // Basic Authorization
            const string username = "Modern";
            const string password = "Warfare";
            var basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            request.AddHeader("Authorization", $"Basic {basicAuthValue}");

            var response = await client.ExecuteAsync(request, cancellationToken);
            if (response.IsSuccessful)
            {
                return response;
            }

            throw new ApiException(
                $"Can not create rest request. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected async Task<RestResponse> SendRequestAsync<T>(T entity, Uri url, Method method,
            CancellationToken cancellationToken)
        {
            var client = new RestClient(SetOptions(url));
            var json = JsonConvert.SerializeObject(entity);
            var request = new RestRequest(url, method);
            // Basic Authorization
            const string username = "Modern";
            const string password = "Warfare";
            var basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            request.AddHeader("Authorization", $"Basic {basicAuthValue}");

            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request, cancellationToken);
            if (response.IsSuccessful)
            {
                return response;
            }

            throw new ApiException(
                $"Can not create rest request. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected RestClientOptions SetOptions(Uri url)
        {
            return new RestClientOptions(url)
            {
                ThrowOnAnyError = true,
                MaxTimeout = _timeout
            };
        }
    }
}