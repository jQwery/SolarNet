using SolaraNet.BusinessLogic.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaraNet.TelegramBot.Commands
{
    public class GetCommand:IBotCommand
    {
        private readonly ITelegramService _telegramService;
        public GetCommand(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }
        public string Command => "ads";

        public string Description => "This is a simple command that can be used to test if the bot is online";

        public bool InternalCommand => false;

        public async Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText)
        {
          var ads=  _telegramService.ListAdvertismentsByWord(commandText);
            foreach (var ad in ads)
            {
                await chatService.SendMessage(chatId, ad.AdvertismentTitle+ad.Description+ad.Price);
            }
            
        }
    }
}
