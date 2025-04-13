using ApplicantTracking.Infrastructure.DataAccess;

using CommonTestUtilities.Cache;
using CommonTestUtilities.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private ApplicantTracking.Domain.Entities.Candidate _cadidate = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicantTrackingDbContext>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<ApplicantTrackingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                StartDatabase(services);

                services.AddScoped(option => new CacheServiceBuilder().Build());
            });
    }

    public int GetCandidadeId() => _cadidate.Id;
    public string GetCandidadeName() => _cadidate.Name;

    private void StartDatabase(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicantTrackingDbContext>();

        context.Database.EnsureDeleted();

        _cadidate = CandidateBuilder.Build();

        context.Candidates.Add(_cadidate);

        context.SaveChanges();
    }
}
