using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicantTracking.Communication.Requests;
using ApplicantTracking.Domain.AuditTrail;
using ApplicantTracking.Domain.Extensions;
using ApplicantTracking.Domain.Repositories;
using ApplicantTracking.Domain.Repositories.Candidate;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;
using AutoMapper;
using FluentValidation.Results;

namespace ApplicantTracking.Application.UseCase.Candidate.Update;

public class UpdateCandidateUseCase : IUpdateCandidateUseCase
{
    private readonly ICandidateCommandRepository _commandRepository;
    private readonly ICandidateQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuditTrailRecorder _auditTrailRecorder;

    public UpdateCandidateUseCase(ICandidateCommandRepository commandRepository, IUnitOfWork unitOfWork, IMapper mapper, IAuditTrailRecorder auditTrailRecorder, ICandidateQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _auditTrailRecorder = auditTrailRecorder;
        _queryRepository = queryRepository;
    }

    public async Task Execute(int id, RequestCandidateJson request)
    {
        var candidate = await _queryRepository.GetById(id) ?? throw new NotFoundException(ResourceMessagesException.CANDIDATE_NOT_FOUND);

        await Validate(request, candidate.Email);        

        _mapper.Map(request, candidate);

        candidate.LastUpdateAt = DateTime.UtcNow;

        _commandRepository.Update(candidate);

        await _unitOfWork.Commit();

        await _auditTrailRecorder.Record(candidate, Domain.Enumerators.TimelineTypes.Update);
    }

    private async Task Validate(RequestCandidateJson request, string currentEmail)
    {
        var result = new CandidateValidator().Validate(request);

        if (currentEmail.Equals(request.Email).IsFalse() && await _queryRepository.ExistCandidateWithEmail(request.Email))
            result.Errors.Add(new ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
