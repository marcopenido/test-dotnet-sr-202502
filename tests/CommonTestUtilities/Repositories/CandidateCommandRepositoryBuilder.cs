using ApplicantTracking.Domain.Repositories.Candidate;

using Moq;

namespace CommonTestUtilities.Repositories;

public class CandidateCommandRepositoryBuilder
{
    public static ICandidateCommandRepository Build()
    {
        var mock = new Mock<ICandidateCommandRepository>();

        return mock.Object;
    }
}
