namespace EA.Common.Exceptions;

public class EmployeeAccountingException : Exception
{
    public EmployeeAccountingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EmployeeAccountingException(string message) : base(message)
    {
    }

    public EmployeeAccountingException(string message, string? errorCode, string? errorMessage)
        : base(message)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }
}