using ApplicantTracking.Application.UseCase.Candidate.Delete;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;

using CommonTestUtilities.AuditTrail;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;

using FluentAssertions;

using Xunit;

namespace UseCases.Test.Candidate.Delete;

public class DeleteCandidateUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var candidate = CandidateBuilder.Build();

        var useCase = CreateUseCase(candidate);

        Func<Task> act = async () => await useCase.Execute(1);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Candidate_Not_Found()
    {
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(1000);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Contains(ResourceMessagesException.CANDIDATE_NOT_FOUND));
    }

    private static DeleteCandidateUseCase CreateUseCase(ApplicantTracking.Domain.Entities.Candidate? candidate = null)
    {
        var commandRepository = CandidateCommandRepositoryBuilder.Build();
        var queryRepository = new CandidateQueryRepositoryBuilder().GetById(candidate).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var auditTrailRecorder = AuditTrailRecorderBuilder.Build();

        return new DeleteCandidateUseCase(commandRepository, unitOfWork, auditTrailRecorder, queryRepository);
    }
}
