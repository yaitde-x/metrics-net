using System.Runtime.Serialization;

namespace MetricsNet
{
    [Serializable]
    internal class StackParsingException : Exception
    {
        public StackParsingException()
        {
        }

        public StackParsingException(string? message) : base(message)
        {
        }

        public StackParsingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected StackParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}