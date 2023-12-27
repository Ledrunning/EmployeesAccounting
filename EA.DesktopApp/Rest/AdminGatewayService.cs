using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Exceptions;
using EA.DesktopApp.Models;
using Newtonsoft.Json;
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

        public async Task<AdministratorModel> ChangeLoginAsync(Credentials credentials, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Administrator/ChangeLogin");
            var response = await SendRequestAsync(credentials, url, Method.Post, token);
            return GetContent<AdministratorModel>(response);
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