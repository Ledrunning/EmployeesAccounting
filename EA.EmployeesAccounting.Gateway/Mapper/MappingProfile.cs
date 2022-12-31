﻿using AutoMapper;
using EA.Repository.Entities;
using EA.ServerGateway.Models;

namespace EA.ServerGateway.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeDto, Employee>();
        CreateMap<AdministratorDto, Administrator>();
    }
}