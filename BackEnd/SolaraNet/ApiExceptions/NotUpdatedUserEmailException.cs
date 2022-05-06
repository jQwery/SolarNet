using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotUpdatedUserEmailException : ConflictException
    {
        public NotUpdatedUserEmailException(string message) : base(message)
        {
        }
    }
}