using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Models;
using RestSharp;

namespace EA.DesktopApp.Rest
{
    public class EmployeeGatewayService : BaseGatewayService, IEmployeeGatewayService
    {
        public EmployeeGatewayService(string baseUrl, int timeout) : base(baseUrl, timeout)
        {
        }

        public async Task<IReadOnlyList<EmployeeModel>> GetAllEmployeeAsync(CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Employee/GetAllEmployee");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<IReadOnlyList<EmployeeModel>>(response);
        }

        public async Task<EmployeeModel> GetByIdAsync(long id, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Employee/GetEmployeeById?id={id}");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<EmployeeModel>(response);
        }

        public async Task<string> GetNameByIdAsync(long id, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Employee/GetEmployeeNameById?id={id}");
            var response = await SendRequestAsync(url, Method.Get, token);

            return GetContent<string>(response);
        }

        public async Task CreateAsync(EmployeeModel employee, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Employee/Create");
            var response = await SendRequestAsync(employee, url, Method.Post, token);
            CheckResponse(response);
        }

        public async Task UpdateAsync(EmployeeModel employee, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Employee/Update");
            var response = await SendRequestAsync(employee, url, Method.Put, token);
            CheckResponse(response);
        }

        public async Task DeleteAsync(long id, CancellationToken token)
        {
            var url = new Uri($"{BaseUrl}/api/Employee/Delete?id={id}");
            var response = await SendRequestAsync(url, Method.Delete, token);
            CheckResponse(response);
        }
    }
}