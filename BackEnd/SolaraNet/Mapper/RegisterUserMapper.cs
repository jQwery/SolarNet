using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.Models;

namespace SolaraNet.Mapper
{
    public class RegisterUserMapper : Profile
    {
        public RegisterUserMapper()
        {
            CreateMap<RegisterUser, UserDTO>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Mail, x => x.MapFrom(y => y.Email))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
                .ForMember(x => x.Password, x => x.MapFrom(y => y.Password))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(y => y.Phone))
                .ForMember(x => x.Role, x => x.MapFrom(x => RoleConstants.UserRole))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}