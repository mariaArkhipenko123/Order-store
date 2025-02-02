using AutoMapper;
using Lab.Application.Models.DTOs.Secure;
using Lab.Domain.Entities;

namespace Lab.Application.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>();  
        }
    }
}
