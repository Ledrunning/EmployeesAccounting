using System;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Exceptions;
using EA.DesktopApp.Models;
using Newtonsoft.Json;
using RestSharp;

namespace EA.DesktopApp.Rest
{
    public class BaseGatewayService
    {
        private readonly int _timeout;
        protected readonly string BaseUrl;
        protected readonly int MaxPingAttempts;
        protected readonly string PingUrl;
        protected readonly int ServerPingTimeout;
        // Store credentials for later use
        public static Credentials Credentials;

        public BaseGatewayService(AppConfig appConfig)
        {
            BaseUrl = appConfig.BaseServerUri;
            _timeout = appConfig.Timeout;
            PingUrl = appConfig.ServerPingUri;
            MaxPingAttempts = appConfig.MaxPingAttempts;
            ServerPingTimeout = appConfig.ServerPingTimeout;
        }

        // Set credentials for the service
        public void SetCredentials(Credentials credentials)
        {
            Credentials = credentials;
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

        protected async Task<RestResponse> SendRequestAsync(Uri url, Method method, 
            CancellationToken token, bool auth = true)
        {
            var client = new RestClient(SetOptions(url));
            var request = new RestRequest(url, method);

            if (auth)
            {
                // Basic Authorization
                var basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Credentials.UserName}:{Credentials.Password}"));
                request.AddHeader("Authorization", $"Basic {basicAuthValue}");
            }

            var response = await client.ExecuteAsync(request, token);
            if (response.IsSuccessful)
            {
                return response;
            }

            throw new ApiException(
                $"Can not create rest request. Status code: {response.StatusCode}, {response.ErrorMessage}");
        }

        protected async Task<RestResponse> SendRequestAsync<T>(
            T entity,
            Uri url,
            Method method,
            CancellationToken token)
        {
            var client = new RestClient(SetOptions(url));
            var request = new RestRequest(url, method);
           
            if (Credentials != null)
            {
                var basicAuthValue =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Credentials.UserName}:{Credentials.Password}"));
                request.AddHeader("Authorization", $"Basic {basicAuthValue}");
            }

            if (method != Method.Get)
            {
                var json = JsonConvert.SerializeObject(entity);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
            }

            var response = await client.ExecuteAsync(request, token);
            if (response.IsSuccessful)
            {
                return response;
            }

            throw new ApiException(
                $"Can not create rest request. Status code: {response.StatusCode}, Error message: {response.ErrorMessage}, Response content: {response.Content}");
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