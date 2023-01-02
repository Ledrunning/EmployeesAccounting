using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA.Repository.Entities;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace EA.Services
{
    public class EmployeeWebApi : BaseService
    {
        public EmployeeWebApi(string baseUrl, int timeout) : base(baseUrl, timeout)
        {
        }

        public async Task<Employee?> GetPersonAsyncOrDefault(Guid id, CancellationToken cancellationToken)
        {
            var url = new Uri($"{BaseUrl}/{id}");
            var client = new RestClient(SetOptions(url));

            var request = new RestRequest();
            var response = await client.ExecuteAsync(request, cancellationToken);

            return GetContent<Employee>(response, url.AbsoluteUri);
        }
    }
}
