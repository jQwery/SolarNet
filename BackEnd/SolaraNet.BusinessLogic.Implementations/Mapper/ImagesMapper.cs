using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations.Mapper
{
    public class ImagesMapper : Profile
    {
        public ImagesMapper()
        {
            CreateMap<DBBaseImage, Image>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.ImageLink, x => x.MapFrom(x => x.ImageLink));
            CreateMap<Image, DBBaseImage>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.ImageLink, x => x.MapFrom(x => x.ImageLink))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}