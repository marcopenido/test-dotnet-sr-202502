using ApplicantTracking.Application.UseCase.Candidate.Update;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;

using CommonTestUtilities.AuditTrail;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

using FluentAssertions;

using Xunit;

namespace UseCases.Test.Candidate.Update;

public class UpdateCandidateUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var candidate = CandidateBuilder.Build();
        var request = RequestCandidateJsonBuilder.Build();

        var useCase = CreateUseCase(candidate);

        Func<Task> act = async () => await useCase.Execute(1, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Candidate_Not_Found()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(1000, request);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Contains(ResourceMessagesException.CANDIDATE_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var candidate = CandidateBuilder.Build();
        var request = RequestCandidateJsonBuilder.Build();

        var useCase = CreateUseCase(candidate, request.Email);

        Func<Task> act = async () => await useCase.Execute(1, request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var candidate = CandidateBuilder.Build();
        var request = RequestCandidateJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(candidate);

        Func<Task> act = async () => await useCase.Execute(1, request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }

    private static UpdateCandidateUseCase CreateUseCase(ApplicantTracking.Domain.Entities.Candidate? candidate = null, string? email = null)
    {
        var commandRepository = CandidateCommandRepositoryBuilder.Build();
        var queryRepository = new CandidateQueryRepositoryBuilder().ExistCandidateWithEmail(email).GetById(candidate).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var auditTrailRecorder = AuditTrailRecorderBuilder.Build();

        return new UpdateCandidateUseCase(commandRepository, unitOfWork, mapper, auditTrailRecorder, queryRepository);
    }
}
