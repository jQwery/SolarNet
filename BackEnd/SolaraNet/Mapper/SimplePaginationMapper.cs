using System;
using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.Models;
using SolaraNet.Models.SpecialModels;

namespace SolaraNet.Mapper
{
    public class SimplePaginationMapper : Profile
    {
        public SimplePaginationMapper()
        {
            CreateMap<SimplePaginationModel, SimplePagination>();
        }
    }
}