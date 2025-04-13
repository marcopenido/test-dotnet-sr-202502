namespace ApplicantTracking.Communication.Requests;

public class RequestCandidateJson
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public string Email { get; set; } = string.Empty;
}
