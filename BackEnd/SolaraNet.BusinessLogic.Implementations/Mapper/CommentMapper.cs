using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations.Mapper
{
    public class CommentMapper : Profile
    {
        public CommentMapper()
        {
            //CreateMap<CommentDTO, DBComment>()
            //    .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
            //    .ForMember(x => x.CommentText, x => x.MapFrom(x => x.CommentText))
            //    .ForMember(x => x.DaysAgo, x => x.MapFrom(x => x.DaysAgo))
            //    .ForMember(x => x.CommentStatus, x => x.Ignore())
            //    .ForMember(x => x.User, x => x.Ignore())
            //    .ForMember(x => x.Advertisment, x => x.Ignore());
            CreateMap<DBComment, CommentDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.CommentText, x => x.MapFrom(x => x.CommentText))
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.User.UserName))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}