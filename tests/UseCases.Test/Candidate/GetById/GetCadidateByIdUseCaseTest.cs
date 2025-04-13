using ApplicantTracking.Application.UseCase.Candidate.GetById;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;

using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;

using FluentAssertions;

using Xunit;

namespace UseCases.Test.Candidate.GetById;

public class GetCadidateByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var candidate = CandidateBuilder.Build();

        var useCase = CreateUseCase(candidate);

        var result = await useCase.Execute(1);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Success_With_Cache()
    {
        var candidate = CandidateBuilder.Build();

        var useCase = CreateUseCase(candidate, true);

        var result = await useCase.Execute(1);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Candidate_Not_Found()
    {
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(1);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Contains(ResourceMessagesException.CANDIDATE_NOT_FOUND));
    }

    private static GetCadidateByIdUseCase CreateUseCase(ApplicantTracking.Domain.Entities.Candidate? candidate = null, bool withChache = false)
    {
        var repository = new CandidateQueryRepositoryBuilder().GetById(candidate).Build();
        var mapper = MapperBuilder.Build();
        var cache = new CacheServiceBuilder();

        if (withChache)
            cache.GetAsync(candidate!);

        return new GetCadidateByIdUseCase(repository, mapper, cache.Build());
    }
}
