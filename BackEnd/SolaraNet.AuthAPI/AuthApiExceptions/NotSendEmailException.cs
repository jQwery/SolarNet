using AbstractsExceptions;

namespace SolaraNet.AuthAPI.AuthApiExceptions
{
    public class NotSendEmailException : ConflictException
    {
        public NotSendEmailException(string message) : base(message)
        {
        }
    }
}