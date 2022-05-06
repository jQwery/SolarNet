using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotUpdatedAdvertismentException : ConflictException
    {
        public NotUpdatedAdvertismentException(string message) : base(message)
        {
        }
    }
}