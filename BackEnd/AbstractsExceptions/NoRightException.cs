namespace AbstractsExceptions
{
    public abstract class NoRightException : DomainException
    {
        protected NoRightException(string message) : base(message)
        {
        }
    }
}