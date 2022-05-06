using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotDeletedAdvertismentException : ConflictException
    {
        public NotDeletedAdvertismentException(string message) : base(message)
        {
        }
    }
}