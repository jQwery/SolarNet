using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotGetAdvertismentCount : NotFoundException
    {
        public NotGetAdvertismentCount(string message) : base(message)
        {
        }
    }
}