using Newtonsoft.Json;
using RestSharp;

namespace EA.Services.GatewayServices;

public class BaseGatewayService
{
    private readonly int _timeout;
    protected readonly string BaseUrl;

    public BaseGatewayService(string baseUrl, int timeout)
    {
        BaseUrl = baseUrl;
        _timeout = timeout;
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

    protected async Task<RestResponse> CreateRestClient(Uri url, CancellationToken cancellationToken)
    {
        var client = new RestClient(SetOptions(url));
        var request = new RestRequest();
        var response = await client.ExecuteAsync(request, cancellationToken);
        if (response.IsSuccessful)
        {
            return response;
        }

        throw new ApplicationException(
            $"Can not create rest request. Status code: {response.StatusCode}, {response.ErrorMessage}");
    }

    protected async Task<RestResponse> CreateRestClient<T>(T entity, Uri url, CancellationToken cancellationToken)
    {
        var client = new RestClient(SetOptions(url));
        var json = JsonConvert.SerializeObject(entity);
        var request = new RestRequest(url, Method.Post);
        request.AddParameter("text/json", json, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request, cancellationToken);
        if (response.IsSuccessful)
        {
            return response;
        }

        throw new ApplicationException(
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