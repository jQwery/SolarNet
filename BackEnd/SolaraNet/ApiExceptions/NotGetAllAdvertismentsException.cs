using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotGetAllAdvertismentsException : NotFoundException
    {
        public NotGetAllAdvertismentsException(string message) : base(message)
        {
        }
    }
}