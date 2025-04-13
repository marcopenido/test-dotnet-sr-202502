using System.Collections.Generic;
using System.Threading.Tasks;

using ApplicantTracking.Communication.Responses;

namespace ApplicantTracking.Application.UseCase.Candidate.GetAll;

public interface IGetAllCadidateUseCase
{
    Task<ResponseCandidatesJson> Execute();
}
