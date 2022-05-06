using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotDeleteUserException : ConflictException
    {
        public NotDeleteUserException(string message) : base(message)
        {
        }
    }
}