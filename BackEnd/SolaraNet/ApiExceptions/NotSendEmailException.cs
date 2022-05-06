using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotSendEmailException : ConflictException
    {
        public NotSendEmailException(string message) : base(message)
        {
        }
    }
}