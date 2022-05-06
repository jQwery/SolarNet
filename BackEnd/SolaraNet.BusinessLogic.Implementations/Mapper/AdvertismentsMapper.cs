using System.Linq.Expressions;
using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;
using Isopoh.Cryptography.Argon2;
using MimeKit.Encodings;

namespace SolaraNet.BusinessLogic.Implementations.Mapper
{
    public class AdvertismentsMapper : Profile
    {
      

        public AdvertismentsMapper()
        {
            // из дто в DB
            CreateMap<AdvertismentDTO, DBAdvertisment>()
                .ForMember(x => x.Image, x => x.Ignore()) // не получится список смапить, да и не всегда нужно
                .ForMember(x => x.User, x => x.Ignore()) // не получится так сделать, потому игнорируем, надо будет по-другому это смапить, вручную
                .ForMember(x => x.Id, x => x.Ignore()) // потому что не всегда и не везде нужно это мапить, например, при создании
                .ForMember(x => x.AdvertismentTitle, x => x.MapFrom(x => x.AdvertismentTitle))
                .ForMember(x => x.Comments, x => x.Ignore()) // в комментариях нет необходимости, они добавляются другим способом
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description))
                .ForMember(x => x.IsPayed, x => x.MapFrom(x => x.IsPayed))
                .ForMember(x => x.Price, x => x.MapFrom(x => x.Price))
                .ForMember(x => x.PublicationDate, x => x.MapFrom(x => x.PublicationDate))
                .ForMember(x => x.Status, x => x.MapFrom(x => EntityStatus.Created))
                .ForMember(x=>x.DBCategoryId, x=>x.MapFrom(x=>x.Category.Id))
                .ForMember(x=>x.City, x=>x.MapFrom(x=>x.City))
                .ForMember(x=>x.DeleteReason, x=>x.MapFrom(x=>x.DeleteReason))
                .ForMember(x=>x.Status, x=>x.MapFrom(x=>x.Status))
                .ForAllOtherMembers(x=>x.Ignore());
            CreateMap<DBAdvertisment, AdvertismentDTO>()
                .ForMember(x => x.AdvertismentID, x => x.MapFrom(x => x.Id))
                .ForPath(x => x.Category.Id, x => x.MapFrom(x=>x.DBCategoryId))
                .ForMember(x => x.AdvertismentTitle, x => x.MapFrom(x => x.AdvertismentTitle))
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description))
                .ForMember(x => x.IsPayed, x => x.MapFrom(x => x.IsPayed))
                .ForMember(x => x.PublicationDate, x => x.MapFrom(x => x.PublicationDate))
                .ForMember(x => x.UserId, x => x.MapFrom(x => x.User.Id))
                .ForMember(x => x.Price, x => x.MapFrom(x => x.Price))
                .ForMember(x=>x.City, x=>x.MapFrom(x=>x.City))
                .ForMember(x=>x.DeleteReason,x=>x.MapFrom(x=>x.DeleteReason))
                .ForMember(x=>x.Status,x=>x.MapFrom(x=>x.Status))
                .ForAllOtherMembers(x=>x.Ignore());
        }
    }
}