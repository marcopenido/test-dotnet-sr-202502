using System.Threading.Tasks;

namespace ApplicantTracking.Application.UseCase.Candidate.Delete;

public interface IDeleteCandidateUseCase
{
    Task Execute(int id);
}
