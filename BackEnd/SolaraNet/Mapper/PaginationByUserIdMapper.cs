using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;

namespace SolaraNet.Mapper
{
    public class PaginationByUserIdMapper : Profile
    {
        public PaginationByUserIdMapper()
        {
            CreateMap<PaginationByUserIdModel, PaginationByUserId>()
                .ForMember(x => x.UsedId, x => x.MapFrom(x => x.UserId))
                .ForMember(x => x.Limit, x => x.MapFrom(x => x.Limit))
                .ForMember(x => x.Offset, x => x.MapFrom(x => x.Offset));
        }
    }
}