using AutoMapper;
using EA.Repository.Entities;
using EA.ServerGateway.Dto;

namespace EA.ServerGateway.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<AdministratorDto, Administrator>();
    }
}