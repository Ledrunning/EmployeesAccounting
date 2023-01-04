using EA.Services.Contracts;
using EA.Services.Models;

namespace EA.Services.GatewayServices;

public class EmployeeGatewayService : BaseGatewayService, IEmployeeGatewayService
{
    public EmployeeGatewayService(string baseUrl, int timeout) : base(baseUrl, timeout)
    {
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetAllEmployee(CancellationToken cancellationToken)
    {
        var url = new Uri($"{BaseUrl}");
        var response = await CreateRestClient(url, cancellationToken);

        return GetContent<IReadOnlyList<EmployeeDto>>(response, url.AbsoluteUri);
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var url = new Uri($"{BaseUrl}/{id}");
        var response = await CreateRestClient(url, cancellationToken);

        return GetContent<EmployeeDto>(response, url.AbsoluteUri);
    }

    public async Task<EmployeeDto?> Create(EmployeeDto employee, CancellationToken cancellationToken)
    {
        var url = new Uri($"{BaseUrl}");
        var response = await CreateRestClient(employee, url, cancellationToken);

        return GetContent<EmployeeDto>(response, url.AbsoluteUri);
    }

    public async Task<EmployeeDto?> Update(EmployeeDto employee, CancellationToken cancellationToken)
    {
        var url = new Uri($"{BaseUrl}");
        var response = await CreateRestClient(employee, url, cancellationToken);

        return GetContent<EmployeeDto>(response, url.AbsoluteUri);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var url = new Uri($"{BaseUrl}/{id}");
        var response = await CreateRestClient(url, cancellationToken);
        GetContent<EmployeeDto>(response, url.AbsoluteUri);
    }
}