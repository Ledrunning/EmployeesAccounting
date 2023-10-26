using EA.DesktopApp.Models;
using System.Threading.Tasks;
using System.Threading;
using System;
using RestSharp;

namespace EA.DesktopApp.Rest
{
    public class AdminGatewayService : BaseGatewayService
    {
        public AdminGatewayService(AppConfig appConfig) : base(appConfig)
        {
        }

        public async Task<EmployeeModel> GetByCredentialsAsync(long id, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/GetByCredentialsAsync?id={id}");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<EmployeeModel>(response);
        }
    }
}