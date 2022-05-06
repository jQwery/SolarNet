using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Mapper
{
    public class ResetEmailMapper : Profile
    {
        public ResetEmailMapper()
        {
            CreateMap<ResetEmailModel, ChangeEmailModel>()
                .ForMember(x => x.NewEmail, x => x.MapFrom(x => x.NewEmail));
        }
    }
}