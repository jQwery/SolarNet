using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotRejectedAdvertismentException : ConflictException
    {
        public NotRejectedAdvertismentException(string message) : base(message)
        {
        }
    }
}