using EA.DesktopApp.Models;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using EA.DesktopApp.Contracts;
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

        public Task<IReadOnlyList<AdministratorModel>> GetAllAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<AdministratorModel> GetByIdAsync(long id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(AdministratorModel admin, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AdministratorModel admin, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}