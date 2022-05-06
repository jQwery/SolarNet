using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;

namespace SolaraNet.Mapper
{
    public class MyPagginationMapper : Profile
    {
        public MyPagginationMapper()
        {
            CreateMap<MyAdvertismentPaginationModel, MyPaginationModel>()
                .ForMember(x => x.HideDeleted, x => x.MapFrom(x => x.HideDeleted))
                .ForMember(x => x.Limit, x => x.MapFrom(x => x.Limit))
                .ForMember(x => x.Offset, x => x.MapFrom(x => x.Offset));
        }
    }
}