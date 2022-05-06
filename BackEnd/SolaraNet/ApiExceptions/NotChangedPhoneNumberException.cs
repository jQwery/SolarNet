using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotChangedPhoneNumberException : ConflictException
    {
        public NotChangedPhoneNumberException(string message) : base(message)
        {
        }
    }
}