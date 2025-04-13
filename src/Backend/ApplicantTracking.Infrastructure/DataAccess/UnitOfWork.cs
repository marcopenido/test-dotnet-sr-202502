using System.Threading.Tasks;
using ApplicantTracking.Domain.Repositories;

namespace ApplicantTracking.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicantTrackingDbContext _context;

    public UnitOfWork(ApplicantTrackingDbContext context)
    {
        _context = context;
    }

    public async Task Commit() => await _context.SaveChangesAsync();
}
