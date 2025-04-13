using System.Threading.Tasks;

namespace ApplicantTracking.Domain.Repositories.Timeline;

public interface ITimelineRepository
{
    Task Add(Entities.Timeline timeline);
    void Update(Entities.Timeline timeline);
    Task<Entities.Timeline> GetByAggregateRootId(int id);
}
