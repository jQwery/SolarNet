using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;

namespace SolaraNet.Mapper
{
    public class ApiUserMapper : Profile
    {
        public ApiUserMapper()
        {
            CreateMap<UserDTO, User>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.Name, x => x.MapFrom(x => x.Name))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(x => x.PhoneNumber))
                .ForMember(x => x.Role, x => x.MapFrom(x => x.Role))
                .ForMember(x => x.AvatarLink, x => x.MapFrom(x => x.Avatar.ImageLink));
        }
    }
}