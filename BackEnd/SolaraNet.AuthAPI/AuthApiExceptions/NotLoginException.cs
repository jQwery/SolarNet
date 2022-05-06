using AbstractsExceptions;

namespace SolaraNet.AuthAPI.AuthApiExceptions
{
    public class NotLoginException : ConflictException
    {
        public NotLoginException(string message) : base(message)
        {
        }
    }
}