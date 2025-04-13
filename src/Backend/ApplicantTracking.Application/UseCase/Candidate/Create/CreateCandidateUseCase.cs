using System.Linq;
using System.Threading.Tasks;
using ApplicantTracking.Communication.Requests;
using ApplicantTracking.Communication.Responses;
using ApplicantTracking.Domain.AuditTrail;
using ApplicantTracking.Domain.Extensions;
using ApplicantTracking.Domain.Repositories;
using ApplicantTracking.Domain.Repositories.Candidate;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;
using AutoMapper;

using FluentValidation.Results;

namespace ApplicantTracking.Application.UseCase.Candidate.Create;

public class CreateCandidateUseCase : ICreateCandidateUseCase
{
    private readonly ICandidateCommandRepository _commandRepository;
    private readonly ICandidateQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditTrailRecorder _auditTrailRecorder;

    public CreateCandidateUseCase(ICandidateCommandRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IAuditTrailRecorder auditTrailRecorder, ICandidateQueryRepository queryRepository)
    {
        _commandRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditTrailRecorder = auditTrailRecorder;
        _queryRepository = queryRepository;
    }

    public async Task<ResponseCandidateJson> Execute(RequestCandidateJson request)
    {
        await Validate(request);

        var candidate = _mapper.Map<Domain.Entities.Candidate>(request);

        await _commandRepository.Add(candidate);

        await _unitOfWork.Commit();

        await _auditTrailRecorder.Record(candidate, Domain.Enumerators.TimelineTypes.Create);

        return _mapper.Map<ResponseCandidateJson>(candidate);
    }

    private async Task Validate(RequestCandidateJson request)
    {
        var result = new CandidateValidator().Validate(request);

        var emailExist = await _queryRepository.ExistCandidateWithEmail(request.Email);

        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
