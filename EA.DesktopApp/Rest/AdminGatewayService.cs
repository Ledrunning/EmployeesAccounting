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

        public async Task<EmployeeModel> SendCredentials(Credentials credentials, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/Login");
            var response = await SendRequestAsync(credentials, url, Method.Get, token);

            var serverToken = response.Content;
            var employee = GetContent<EmployeeModel>(response);
            employee.Token = serverToken;
            return employee;
        }
    }
}