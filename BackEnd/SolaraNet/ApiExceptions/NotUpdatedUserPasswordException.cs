using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotUpdatedUserPasswordException : ConflictException
    {
        public NotUpdatedUserPasswordException(string message) : base(message)
        {
        }
    }
}