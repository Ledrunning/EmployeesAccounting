﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Models;

namespace EA.DesktopApp.Rest
{
    /// <summary>
    ///     Client class for REST Web API
    ///     HTTP Client Nuget needed
    /// </summary>
    public class EmployeeApi : IEmployeeApi
    {
        private const string ApiName = "api/employee/";
        private readonly string _baseAddress;

        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="baseAddress"></param>
        public EmployeeApi(string baseAddress)
        {
            this._baseAddress = baseAddress;
        }

        /// <summary>
        ///     Method for getting persons from data base by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmployeeModel> GetPersonAsync(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync($"{ApiName}{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var person = await response.Content.ReadAsAsync<EmployeeModel>();
                        return person;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            return null;
        }

        /// <summary>
        ///     Get all persons from data base
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeModel>> GetAllAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try

                {
                    var response = await client.GetAsync(ApiName);
                    if (response.IsSuccessStatusCode)
                    {
                        var person = await response.Content.ReadAsAsync<IEnumerable<EmployeeModel>>();
                        return person;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            return null;
        }

        /// <summary>
        ///     Method for adding person to data base
        /// </summary>
        /// <param name="person"></param>
        public async void AddPerson(EmployeeModel person)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.PostAsJsonAsync($"{ApiName}", person);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Error when adding file!");
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public bool UpdatePerson(EmployeeModel employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var index = empContext.Employee.FirstOrDefault(c => c.Id == employee.Id);
                //if (index != null)
                //{
                //    return false;
                //}
                //empContext.Employee.Remove(index);
                //empContext.Employee.Add(employee);
                return true;
            }
        }
    }
}