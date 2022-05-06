using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotCreatedCommentException : ConflictException
    {
        public NotCreatedCommentException(string message) : base(message)
        {
        }
    }
}