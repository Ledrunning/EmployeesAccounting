using System;

namespace EA.RecognizerEngine.Exceptions
{
    public class RecognizerEngineException : Exception
    {
        public RecognizerEngineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RecognizerEngineException(string message) : base(message)
        {
        }

        public RecognizerEngineException(string message, string errorCode, string errorMessage)
            : base(message)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public string ErrorCode { get; }

        public string ErrorMessage { get; }
    }
}