using AutoMapper;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.BusinessLogic.Abstracts;

namespace SolaraNet.BusinessLogic.Implementations
{
   public class TelegramService:ITelegramService
    {
        #region Поля
        private readonly ITelegramRepository _telegramRepository;
        private readonly ISaver _saver;
        private readonly IMapper _mapper;

        #endregion
        public TelegramService(ITelegramRepository telegramRepository, ISaver saver, IMapper mapper)
        {
            _telegramRepository = telegramRepository;
            _saver = saver;
            _mapper = mapper;
        }
        public  IEnumerable<AdvertismentDTO>ListAdvertismentsByWord( string word)
        {

            var advertisments= OperationResult<IEnumerable<AdvertismentDTO>>.Ok(_mapper.Map< IEnumerable < AdvertismentDTO >> ( _telegramRepository.ListAdvertismentsByWord(word)));
            if (advertisments == null)
            {
                throw new Exception();
            }
            return (IEnumerable<AdvertismentDTO>)advertisments;
        }
    }
}
