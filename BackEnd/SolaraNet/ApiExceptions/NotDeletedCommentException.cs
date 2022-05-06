using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NotDeletedCommentException : ConflictException
    {
        public NotDeletedCommentException(string message) : base(message)
        {
        }
    }
}