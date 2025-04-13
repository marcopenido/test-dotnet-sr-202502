namespace ApplicantTracking.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : ApplicantTrackingException
{
    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        ErrorMessages = errorMessages;
    }

    public IList<string> ErrorMessages { get; set; }
}
