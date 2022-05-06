using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotChangedUserNameException : ConflictException
    {
        public NotChangedUserNameException(string message) : base(message)
        {
        }
    }
}