using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<DBUser, UserDTO>()
                .ForPath(x => x.Avatar.ImageLink, x => x.MapFrom(x => x.Avatar.ImageLink))
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(x => x.PhoneNumber))
                .ForMember(x => x.Mail, x => x.MapFrom(x => x.Email))
                .ForMember(x => x.Name, x => x.MapFrom(x => x.UserName))
                .ForMember(x => x.Password, x => x.MapFrom(x => x.PasswordHash))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}