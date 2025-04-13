using System.Threading.Tasks;
using ApplicantTracking.Domain.AuditTrail;
using ApplicantTracking.Domain.Repositories;
using ApplicantTracking.Domain.Repositories.Candidate;
using ApplicantTracking.Exceptions;
using ApplicantTracking.Exceptions.ExceptionsBase;

namespace ApplicantTracking.Application.UseCase.Candidate.Delete;

public class DeleteCandidateUseCase : IDeleteCandidateUseCase
{
    private readonly ICandidateCommandRepository _commandRepository;
    private readonly ICandidateQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditTrailRecorder _auditTrailRecorder;

    public DeleteCandidateUseCase(ICandidateCommandRepository commandRepository, IUnitOfWork unitOfWork, IAuditTrailRecorder auditTrailRecorder, ICandidateQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
        _auditTrailRecorder = auditTrailRecorder;
        _queryRepository = queryRepository;
    }

    public async Task Execute(int id)
    {
        var candidate = await _queryRepository.GetById(id) ?? throw new NotFoundException(ResourceMessagesException.CANDIDATE_NOT_FOUND);

        await _commandRepository.Delete(candidate.Id);

        await _unitOfWork.Commit();

        await _auditTrailRecorder.Record(candidate, Domain.Enumerators.TimelineTypes.Delete);
    }
}
