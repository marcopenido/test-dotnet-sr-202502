using System.Collections.Generic;
using System.Threading.Tasks;

using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Repositories.Candidate;

using Microsoft.EntityFrameworkCore;

namespace ApplicantTracking.Infrastructure.DataAccess.Repositories;

public class CandidateRepository : ICandidateQueryRepository, ICandidateCommandRepository
{
    private readonly ApplicantTrackingDbContext _context;

    public CandidateRepository(ApplicantTrackingDbContext context)
    {
        _context = context;
    }

    public async Task Add(Candidate candidate)
    {
        await _context.Candidates.AddAsync(candidate);
    }

    public async Task Delete(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);

        _context.Candidates.Remove(candidate!);
    }

    public async Task<bool> ExistCandidateWithEmail(string email)
    {
        return await _context.Candidates
            .AsNoTracking()
            .AnyAsync(c => c.Email.Equals(email));
    }

    public async Task<IEnumerable<Candidate>> GetAll()
    {
        return await _context.Candidates
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Candidate> GetById(int id)
    {
        return await _context.Candidates
             .FirstOrDefaultAsync(c => c.Id == id);
    }

    public void Update(Candidate candidate)
    {
        _context.Update(candidate);
    }
}
