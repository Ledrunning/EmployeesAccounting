﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EA.DesktopApp.Models;

namespace EA.DesktopApp.Contracts
{
    public interface IEmployeeApi
    {
        /// <summary>
        ///     Method for getting persons from data base by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Person> GetPersonAsync(Guid id);

        /// <summary>
        ///     Get all persons from data base
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Person>> GetAllAsync();

        /// <summary>
        ///     Method for adding person to data base
        /// </summary>
        /// <param name="person"></param>
        void AddPerson(Person person);

        bool UpdatePerson(Person employee);
    }
}