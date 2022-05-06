using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class IncorrectCodeException : ConflictException
    {
        public IncorrectCodeException(string message) : base(message)
        {
        }
    }
}