using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotBannedUserException : ConflictException
    {
        public NotBannedUserException(string message) : base(message)
        {
        }
    }
}