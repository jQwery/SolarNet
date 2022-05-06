using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotApprovedException : ConflictException
    {
        public NotApprovedException(string message) : base(message)
        {
        }
    }
}