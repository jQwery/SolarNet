using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotCreatedAdvertismentException : ConflictException
    {
        public NotCreatedAdvertismentException(string message) : base(message)
        {
        }
    }
}