using System.Threading.Tasks;

using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Repositories.Timeline;

using Microsoft.EntityFrameworkCore;

namespace ApplicantTracking.Infrastructure.DataAccess.Repositories;

public class TimelineRepository : ITimelineRepository
{
    private readonly ApplicantTrackingDbContext _context;

    public TimelineRepository(ApplicantTrackingDbContext context)
    {
        _context = context;
    }

    public async Task Add(Timeline timeline)
    {
        await _context.Timelines.AddAsync(timeline);
    }

    public async Task<Timeline> GetByAggregateRootId(int id)
    {
        return await _context.Timelines.FirstOrDefaultAsync(t => t.IdAggregateRoot == id);
    }

    public void Update(Timeline timeline)
    {
        _context.Timelines.Update(timeline);
    }
}
