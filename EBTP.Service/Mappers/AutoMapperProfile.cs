using AutoMapper;
using EBTP.Repository.Entities;
using EBTP.Service.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.Mappers
{
    public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //User
        CreateMap<User, UserDTO>().ReverseMap();
    }
}
}
