using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class IncorrectPasswordException : ConflictException
    {
        public IncorrectPasswordException(string message) : base(message)
        {
        }
    }
}