using System;
using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.Models;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Mapper
{
    public class AdvertismentApiMapper : Profile
    {
        public AdvertismentApiMapper()
        {
            CreateMap<AdvertismentFromFront, AdvertismentDTO>()
                .ForMember(x => x.AdvertismentTitle, x => x.MapFrom(x => x.AdvertismentTitle))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description))
                .ForMember(x => x.Price, x => x.MapFrom(x => x.Price))
                .ForPath(x   => x.Category.Id, x => x.MapFrom(x => x.CategoryId))
                .ForMember(x => x.IsPayed, x => x.MapFrom(x => false))
                .ForMember(x => x.IsNew, x => x.MapFrom(x => true))
                .ForMember(x => x.PublicationDate, x => x.MapFrom(x => DateTime.Today))
                .ForMember(x => x.City, x => x.MapFrom(x => x.City))
                .ForMember(x=>x.DeleteReason, x=>x.MapFrom(x=>""))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<AdvertismentDTO, AdvertismentForFront>()
                .ForMember(x => x.AdvertismentTitle, x => x.MapFrom(x => x.AdvertismentTitle))
                .ForMember(x => x.Price, x => x.MapFrom(x => x.Price))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description))
                .ForMember(x => x.Date,
                    x => x.MapFrom(x =>
                        x.PublicationDate.Day + "." + x.PublicationDate.Month + "." + x.PublicationDate.Year))
                .ForMember(x => x.Id, x => x.MapFrom(x => x.AdvertismentID))
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.UserName))
                .ForMember(x => x.Images, x => x.Ignore())
                .ForMember(x => x.CategoryId, x => x.MapFrom(x => x.Category.Id))
                .ForMember(x => x.City, x => x.MapFrom(x => x.City))
                .ForMember(x => x.UserId, x=>x.MapFrom(x=>x.UserId))
                .ForMember(x => x.UserPhone, x=>x.MapFrom(x=>x.UserPhone))
                .ForMember(x => x.DeleteReason, x=>x.MapFrom(x=>x.DeleteReason))
                .ForMember(x => x.Status,x=>x.MapFrom(x=>x.Status.ToString()))
                .ForMember(x=>x.CommentsCount, x=> x.MapFrom(x=> x.CommentCount))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}