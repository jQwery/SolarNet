using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotFoundAdvertismentException : NotFoundException
    {
        public NotFoundAdvertismentException(string message) : base(message)
        {
        }
    }
}