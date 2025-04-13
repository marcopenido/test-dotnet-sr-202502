using System.Threading.Tasks;
using ApplicantTracking.Communication.Requests;

namespace ApplicantTracking.Application.UseCase.Candidate.Update;

public interface IUpdateCandidateUseCase
{
    Task Execute(int id, RequestCandidateJson request);
}
