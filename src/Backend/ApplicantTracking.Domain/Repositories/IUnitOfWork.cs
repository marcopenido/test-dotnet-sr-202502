using System.Threading.Tasks;

namespace ApplicantTracking.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
