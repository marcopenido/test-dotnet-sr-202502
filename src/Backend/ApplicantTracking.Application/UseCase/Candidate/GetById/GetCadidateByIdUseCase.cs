using System.Threading.Tasks;
using ApplicantTracking.Communication.Responses;
using ApplicantTracking.Domain.Repositories.Candidate;
using ApplicantTracking.Domain.Services.Caching;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;
using AutoMapper;

namespace ApplicantTracking.Application.UseCase.Candidate.GetById;

public class GetCadidateByIdUseCase : IGetCadidateByIdUseCase
{
    private readonly ICandidateQueryRepository _queryRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetCadidateByIdUseCase(ICandidateQueryRepository candidateRepository, IMapper mapper, ICacheService cache)
    {
        _queryRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ResponseCandidateJson> Execute(int id)
    {
        var cacheKey = $"getById-candidate-{id}";

        var candidate = await _cache.GetAsync<Domain.Entities.Candidate>(cacheKey);

        if (candidate is null)
        {
            candidate = await _queryRepository.GetById(id) ?? throw new NotFoundException(ResourceMessagesException.CANDIDATE_NOT_FOUND);

            await _cache.SetAsync(cacheKey, candidate);
        }

        return _mapper.Map<ResponseCandidateJson>(candidate);
    }
}
