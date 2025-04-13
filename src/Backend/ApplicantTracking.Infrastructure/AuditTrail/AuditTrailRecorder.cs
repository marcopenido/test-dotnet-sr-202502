using System;
using System.Text.Json;
using System.Threading.Tasks;

using ApplicantTracking.Domain.AuditTrail;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Entities.Base;
using ApplicantTracking.Domain.Enumerators;
using ApplicantTracking.Domain.Repositories;
using ApplicantTracking.Domain.Repositories.Timeline;

namespace ApplicantTracking.Infrastructure.AuditTrail;

public class AuditTrailRecorder : IAuditTrailRecorder
{
    private readonly ITimelineRepository _timelineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuditTrailRecorder(ITimelineRepository timelineRepository, IUnitOfWork unitOfWork)
    {
        _timelineRepository = timelineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Record<T>(T entity, TimelineTypes type) where T : EntityBase
    {
        var serializedEntity = JsonSerializer.Serialize(entity);
        var timeline = await _timelineRepository.GetByAggregateRootId(entity.Id);

        var newTimeline = new Timeline
        {
            IdTimelineType = type,
            IdAggregateRoot = entity.Id,
            CreatedAt = DateTime.UtcNow
        };

        switch (type)
        {
            case TimelineTypes.Create:
                newTimeline.OldData = string.Empty;
                newTimeline.NewData = serializedEntity;
                break;

            case TimelineTypes.Update:
                var oldData = string.Empty;
                if (timeline != null)
                    oldData = timeline.NewData;
                newTimeline.OldData = oldData;
                newTimeline.NewData = serializedEntity;
                break;

            case TimelineTypes.Delete:
                newTimeline.OldData = serializedEntity;
                newTimeline.NewData = null;
                break;

            default:
                return;
        }

        await _timelineRepository.Add(newTimeline);

        await _unitOfWork.Commit();
    }
}
