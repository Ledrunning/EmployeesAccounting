﻿using EA.Services.Contracts;
using EA.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers;

[Authorize]
public class AdministratorController : Controller
{
    private readonly IAdministratorService _administratorService;

    public AdministratorController(IAdministratorService administratorService)
    {
        _administratorService = administratorService;
    }

    public IActionResult Index()
    {
        return Content("Welcome to EmployeesAccounting administrator web api page!");
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task Create([FromBody] AdministratorDto administrator, CancellationToken cancellationToken)
    {
        await _administratorService.AddAsync(administrator, cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetAllAdministrators))]
    public async Task<IReadOnlyList<AdministratorDto?>> GetAllAdministrators(CancellationToken cancellationToken)
    {
        return await _administratorService.GetAllAsync(cancellationToken);
    }

    [HttpGet]
    [Route(nameof(GetAdministratorById))]
    public async Task<AdministratorDto?> GetAdministratorById(int id, CancellationToken cancellationToken)
    {
        return await _administratorService.GetByIdAsync(id, cancellationToken);
    }

    [HttpGet]
    [Route(nameof(Login))]
    public async Task<AdministratorDto?> Login(string login, string pass, CancellationToken cancellationToken)
    {
        return await _administratorService.GetByCredentialsAsync(login, pass, cancellationToken);
    }

    [HttpPut]
    [Route(nameof(Update))]
    public async Task Update(AdministratorDto administrator, CancellationToken cancellationToken)
    {
        await _administratorService.UpdateAsync(administrator, cancellationToken);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await _administratorService.DeleteAsync(id, cancellationToken);
    }
}