using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotAddedNewUser : ConflictException
    {
        public NotAddedNewUser(string message) : base(message)
        {
        }
    }
}