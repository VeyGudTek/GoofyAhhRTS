using System;

namespace Source.Shared.Utilities
{
    [Serializable]
    public class InitializationException : Exception
    {
        public InitializationException()
        { }

        public InitializationException(string message)
            : base(message)
        { }

        public InitializationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
