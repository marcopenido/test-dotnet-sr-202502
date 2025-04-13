using System.Threading.Tasks;

namespace ApplicantTracking.Domain.Repositories.Candidate;

public interface ICandidateCommandRepository
{
    Task Add(Entities.Candidate candidate);
    Task Delete(int id);
    void Update(Entities.Candidate candidate);
}
