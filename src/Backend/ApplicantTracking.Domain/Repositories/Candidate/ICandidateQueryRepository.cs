using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicantTracking.Domain.Repositories.Candidate;

public interface ICandidateQueryRepository
{
    Task<IEnumerable<Entities.Candidate>> GetAll();
    Task<Entities.Candidate> GetById(int id);
    Task<bool> ExistCandidateWithEmail(string email);
}
