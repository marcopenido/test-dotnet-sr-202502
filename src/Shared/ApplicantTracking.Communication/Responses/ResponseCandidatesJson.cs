namespace ApplicantTracking.Communication.Responses
{
    public class ResponseCandidatesJson
    {
        public IList<ResponseCandidateJson> Candidates { get; set; } = new List<ResponseCandidateJson>();
    }
}
