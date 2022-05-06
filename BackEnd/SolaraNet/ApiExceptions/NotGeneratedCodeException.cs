using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotGeneratedCodeException : ConflictException
    {
        public NotGeneratedCodeException(string message) : base(message)
        {
        }
    }
}