using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EA.TestClientForm.Model;

namespace EA.TestClientForm.Helpers
{
    public class WebApiSender
    {
        private readonly string _baseAddress;

        public WebApiSender(string baseAddress)
        {
            this._baseAddress = baseAddress;
        }

        public async Task<Person> GetPersonAsync(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                response = await client.GetAsync($"api/employee/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var person = await response.Content.ReadAsAsync<Person>();
                    return person;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                response = await client.GetAsync("api/employee/");
                if (response.IsSuccessStatusCode)
                {
                    var person = await response.Content.ReadAsAsync<IEnumerable<Person>>();
                    return person;
                }
            }

            return null;
        }

        public async Task AddPerson(Person person)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("api/employee/", person);
                if (!response.IsSuccessStatusCode) throw new Exception("Error when adding file!");
            }
        }

        public Person SearchEmploeeById(string id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"api/employee/{id}";

            using (var response = client.GetAsync(url).GetAwaiter().GetResult())
            {
                if (response.IsSuccessStatusCode)
                    try
                    {
                        var employees = response.Content.ReadAsAsync<Person>().GetAwaiter().GetResult();
                        return employees;
                    }
                    catch (Exception err)
                    {
                        throw;
                    }

                throw new NullReferenceException();
            }
        }
    }
}