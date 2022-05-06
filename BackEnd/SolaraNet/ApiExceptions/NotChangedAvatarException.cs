using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotChangedAvatarException : ConflictException
    {
        public NotChangedAvatarException(string message) : base(message)
        {
        }
    }
}