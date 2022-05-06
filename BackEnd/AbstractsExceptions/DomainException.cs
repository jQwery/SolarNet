using System;

namespace AbstractsExceptions
{
    public abstract class DomainException : ApplicationException
    {
        protected DomainException(string message) : base(message)
        {
        }
    }
}
