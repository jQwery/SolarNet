using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotFoundUserException : NotFoundException
    {
        public NotFoundUserException(string message) : base(message)
        {
        }
    }
}