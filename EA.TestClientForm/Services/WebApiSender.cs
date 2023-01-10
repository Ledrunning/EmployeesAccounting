using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EA.TestClientForm.Model;

namespace EA.TestClientForm.Services
{
    public class WebApiSender
    {
        private readonly string _baseAddress;

        public WebApiSender(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public async Task<Employee> GetPersonAsyncOrDefault(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"api/employee/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var person = await response.Content.ReadAsAsync<Employee>();
                return person;
            }
        }

        public async Task<IEnumerable<Employee>> GetAllPersonsAsyncOrDefault()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/employee/");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var person = await response.Content.ReadAsAsync<IEnumerable<Employee>>();
                return person;
            }
        }

        public async Task AddPerson(Employee employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("api/employee/", employee);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error when adding file!");
                }
            }
        }

        public async Task<Employee> SearchEmployeeByIdOrDefault(string id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"api/employee/{id}";

            using (var response = await client.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                {
                    var employees = await response.Content.ReadAsAsync<Employee>();
                    return employees;
                }

            }
        }
    }
}