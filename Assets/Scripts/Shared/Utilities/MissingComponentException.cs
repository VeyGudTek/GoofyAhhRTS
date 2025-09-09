using System;

namespace Source.Shared.Utilities
{
    [Serializable]
    public class MissingComponentException : Exception
    {
        public MissingComponentException()
        { }

        public MissingComponentException(string message)
            : base(message)
        { }

        public MissingComponentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}