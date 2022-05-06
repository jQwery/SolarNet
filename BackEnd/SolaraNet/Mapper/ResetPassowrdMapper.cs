using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Mapper
{
    public class ResetPassowrdMapper : Profile
    {
        public ResetPassowrdMapper()
        {
            CreateMap<ResetPasswordModel, ChangePasswordModel>()
                .ForMember(x => x.CurrentPassword, x => x.MapFrom(x => x.CurrentPassword))
                .ForMember(x => x.NewPassword, x => x.MapFrom(x => x.NewPassword));
        }
    }
}