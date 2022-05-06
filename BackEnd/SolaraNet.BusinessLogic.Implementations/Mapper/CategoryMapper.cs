using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryDTO, DBCategory>()
                .ForMember(x => x.UnderCategories, x => x.Ignore())
                .ForMember(x => x.Advertisments, x => x.Ignore())
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<DBCategory, CategoryDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id));
        }
    }
}