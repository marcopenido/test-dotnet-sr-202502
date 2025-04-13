namespace ApplicantTracking.Communication.Responses;

public class ResponseCandidateJson
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public string Email { get; set; } = string.Empty;
}
