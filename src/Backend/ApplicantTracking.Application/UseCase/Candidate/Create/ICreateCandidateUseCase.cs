using System.Threading.Tasks;
using ApplicantTracking.Communication.Requests;
using ApplicantTracking.Communication.Responses;

namespace ApplicantTracking.Application.UseCase.Candidate.Create;

public interface ICreateCandidateUseCase
{
    Task<ResponseCandidateJson> Execute(RequestCandidateJson request);
}
