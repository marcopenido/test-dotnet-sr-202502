namespace ApplicantTracking.Exceptions.ExceptionsBase;

public class NotFoundException : ApplicantTrackingException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
