using System.Threading.Tasks;

using ApplicantTracking.Domain.Entities.Base;
using ApplicantTracking.Domain.Enumerators;

namespace ApplicantTracking.Domain.AuditTrail;

public interface IAuditTrailRecorder
{
    Task Record<T>(T entity, TimelineTypes actionType) where T : EntityBase;
}
