using System;

namespace Exception
{
    public abstract class DomainException : ApplicationException
    {
        protected DomainException(string message): base(message) {}
    }
}