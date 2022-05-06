using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations.Mapper
{
    public class PaginationMapper : Profile
    {
        public PaginationMapper()
        {
            CreateMap<SimplePagination, PaginationModel>().ForMember(x => x.Limit, x => x.MapFrom(x => x.Limit))
                .ForMember(x => x.Offset, x => x.MapFrom(x => x.Offset))
                .ForMember(x => x.ByDescendingCost, x => x.MapFrom(x => false))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}