using System.Threading.Tasks;
using ApplicantTracking.Communication.Responses;

namespace ApplicantTracking.Application.UseCase.Candidate.GetById;

public interface IGetCadidateByIdUseCase
{
    Task<ResponseCandidateJson> Execute(int id);
}
