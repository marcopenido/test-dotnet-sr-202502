using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Repositories.Candidate;

using Moq;

namespace CommonTestUtilities.Repositories;

public class CandidateQueryRepositoryBuilder
{
    private readonly Mock<ICandidateQueryRepository> _repository;

    public CandidateQueryRepositoryBuilder() => _repository = new Mock<ICandidateQueryRepository>();

    public CandidateQueryRepositoryBuilder GetAll(IEnumerable<Candidate>? candidates)
    {
        if (candidates is not null)
            _repository.Setup(r => r.GetAll()).ReturnsAsync(candidates);

        return this;
    }

    public CandidateQueryRepositoryBuilder GetById(Candidate? candidate)
    {
        if (candidate is not null)
            _repository.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync(candidate);

        return this;
    }

    public CandidateQueryRepositoryBuilder ExistCandidateWithEmail(string? email)
    {
        if (email is not null)
            _repository.Setup(r => r.ExistCandidateWithEmail(email)).ReturnsAsync(true);

        return this;
    }

    public ICandidateQueryRepository Build() => _repository.Object;
}
