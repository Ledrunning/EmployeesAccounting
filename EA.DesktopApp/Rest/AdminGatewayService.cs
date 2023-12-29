using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Models;
using RestSharp;

namespace EA.DesktopApp.Rest
{
    public class AdminGatewayService : BaseGatewayService, IAdminGatewayService
    {
        public AdminGatewayService(AppConfig appConfig) : base(appConfig)
        {
        }

        public async Task<bool> Login(CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/Login");
            var response = await SendRequestAsync(Credentials, url, Method.Post, token);
            return GetContent<bool>(response);
        }

        public async Task<bool> Login(Credentials credentials, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/Login");
            var response = await SendRequestAsync(credentials, url, Method.Post, token);
            return GetContent<bool>(response);
        }

        public async Task<IReadOnlyList<AdministratorModel>> GetAllAsync(CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/GetAllAdministrators");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<IReadOnlyList<AdministratorModel>>(response);
        }

        public async Task<AdministratorModel> GetByIdAsync(long id, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/GetAdministratorById?id={id}");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<AdministratorModel>(response);
        }

        public async Task<bool> ChangeLoginAsync(Credentials credentials, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/ChangeLogin");
            var response = await SendRequestAsync(credentials, url, Method.Post, token);
            return GetContent<bool>(response);
        }

        public async Task CreateAsync(AdministratorModel admin, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/Create");
            var response = await SendRequestAsync(admin, url, Method.Post, token);
            CheckResponse(response);
        }

        public async Task UpdateAsync(AdministratorModel admin, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/Update");
            var response = await SendRequestAsync(admin, url, Method.Put, token);
            CheckResponse(response);
        }

        public async Task DeleteAsync(long id, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/Delete?id={id}");
            var response = await SendRequestAsync(url, Method.Delete, token);
            CheckResponse(response);
        }
    }
}