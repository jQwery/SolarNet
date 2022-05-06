namespace AbstractsExceptions
{
    public abstract class ConflictException : DomainException
    {
        protected ConflictException(string message): base(message)
        {
        }
    }
}