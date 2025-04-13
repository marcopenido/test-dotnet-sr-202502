using ApplicantTracking.Application.UseCase.Candidate.GetAll;

using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;

using FluentAssertions;

using Xunit;

namespace UseCases.Test.Candidate.GetAll;

public class GetAllCadidateUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var candidates = CandidateBuilder.Collection();

        var useCase = CreateUseCase(candidates);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Candidates.Should().NotBeNullOrEmpty();
        result.Candidates.Should().HaveCount(candidates.Count);
    }

    [Fact]
    public async Task Success_With_Cache()
    {
        var candidates = CandidateBuilder.Collection();

        var useCase = CreateUseCase(candidates, true);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Candidates.Should().NotBeNullOrEmpty();
        result.Candidates.Should().HaveCount(candidates.Count);
    }

    private static GetAllCadidateUseCase CreateUseCase(IEnumerable<ApplicantTracking.Domain.Entities.Candidate>? candidates = null, bool withChache = false)
    {
        var repository = new CandidateQueryRepositoryBuilder().GetAll(candidates!).Build();
        var mapper = MapperBuilder.Build();
        var cache = new CacheServiceBuilder();

        if (withChache)
            cache.GetAsync(candidates!);

        return new GetAllCadidateUseCase(repository, mapper, cache.Build());
    }
}
