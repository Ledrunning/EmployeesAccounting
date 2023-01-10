using System;
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
        Task<EmployeeModel> GetPersonAsync(Guid id);

        /// <summary>
        ///     Get all persons from data base
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EmployeeModel>> GetAllAsync();

        /// <summary>
        ///     Method for adding person to data base
        /// </summary>
        /// <param name="person"></param>
        void AddPerson(EmployeeModel person);

        bool UpdatePerson(EmployeeModel employee);
    }
}