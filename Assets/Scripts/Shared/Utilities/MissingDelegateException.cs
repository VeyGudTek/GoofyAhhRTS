using System;

namespace Source.Shared.Utilities
{
    [Serializable]
    public class MissingDelegateException : Exception
    {
        public MissingDelegateException()
        { }

        public MissingDelegateException(string message)
            : base(message)
        { }

        public MissingDelegateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}