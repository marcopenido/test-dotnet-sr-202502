using ApplicantTracking.Application.UseCase.Candidate.Create;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;

using CommonTestUtilities.AuditTrail;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

using FluentAssertions;

using Xunit;

namespace UseCases.Test.Candidate.Create;

public class CreateCandidateUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                        e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }

    private static CreateCandidateUseCase CreateUseCase(string? email = null)
    {
        var commandRepository = CandidateCommandRepositoryBuilder.Build();
        var queryRepository = new CandidateQueryRepositoryBuilder().ExistCandidateWithEmail(email).Build();  
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var auditTrailRecorder = AuditTrailRecorderBuilder.Build();

        return new CreateCandidateUseCase(commandRepository, unitOfWork, mapper, auditTrailRecorder, queryRepository);
    }
}
