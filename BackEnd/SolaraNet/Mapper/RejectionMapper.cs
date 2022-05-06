using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;

namespace SolaraNet.Mapper
{
    public class RejectionMapper : Profile
    {
        public RejectionMapper()
        {
            CreateMap<RejectionModelApi, RejectionModel>()
                .ForMember(x => x.AdvertismentId, x => x.MapFrom(x => x.AdvertismentId))
                .ForMember(x => x.DeleteReason, x => x.MapFrom(x => x.DeleteReason));
        }
    }
}