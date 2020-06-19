using AutoMapper;
using NetCoreAPI.Dtos;
using NetCoreAPI.Models;

namespace NetCoreAPI.AutoMapperProfile
{
    public class EmployeeProfile: Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}