using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicantTracking.Communication.Responses;
using ApplicantTracking.Domain.Repositories.Candidate;
using ApplicantTracking.Domain.Services.Caching;
using AutoMapper;

namespace ApplicantTracking.Application.UseCase.Candidate.GetAll;

public class GetAllCadidateUseCase : IGetAllCadidateUseCase
{
    private readonly ICandidateQueryRepository _queryRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetAllCadidateUseCase(ICandidateQueryRepository candidateRepository, IMapper mapper, ICacheService cache)
    {
        _queryRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ResponseCandidatesJson> Execute()
    {
        var cacheKey = $"getAll-candidate";

        var candidates = await _cache.GetAsync<IEnumerable<Domain.Entities.Candidate>>(cacheKey);

        if (candidates is null || !candidates.Any())
        {
            candidates = await _queryRepository.GetAll();

            await _cache.SetAsync(cacheKey, candidates);
        }

        return _mapper.Map<ResponseCandidatesJson>(candidates);
    }
}
