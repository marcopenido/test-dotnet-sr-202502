using ApplicantTracking.Domain.Entities.Base;
using ApplicantTracking.Domain.Enumerators;

namespace ApplicantTracking.Domain.Entities;

public class Timeline : EntityBase
{
    public TimelineTypes IdTimelineType { get; set; }
    public int IdAggregateRoot { get; set; }
    public string OldData { get; set; }
    public string NewData { get; set; }
}
