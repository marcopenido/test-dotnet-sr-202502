using System;
using ApplicantTracking.Domain.AuditTrail;
using ApplicantTracking.Domain.Extensions;
using ApplicantTracking.Domain.Repositories;
using ApplicantTracking.Domain.Repositories.Candidate;
using ApplicantTracking.Domain.Repositories.Timeline;
using ApplicantTracking.Domain.Services.Caching;
using ApplicantTracking.Infrastructure.AuditTrail;
using ApplicantTracking.Infrastructure.DataAccess;
using ApplicantTracking.Infrastructure.DataAccess.Repositories;
using ApplicantTracking.Infrastructure.Extensions;
using ApplicantTracking.Infrastructure.Services.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicantTracking.Infrastructure
{
    public static class InfrastructureDependecyInjection
    {
        public static void AddDbContextSqlServer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicantTrackingDbContext>(options =>
            {
                options.UseSqlServer(configuration.ConnectionString());
            });
        }

        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICandidateQueryRepository, CandidateRepository>();
            services.AddScoped<ICandidateCommandRepository, CandidateRepository>();
            services.AddScoped<ITimelineRepository, TimelineRepository>();
        }

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicantTrackingDbContext>();
            dbContext.Database.Migrate();
        }

        public static void AddRedis(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICacheService, RedisCacheService>();

            var redisConnectionString = configuration.RedisConnectionString();

            if (redisConnectionString.NotEmpty())
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                });
            }
        }

        public static void AddAuditTrail(IServiceCollection services)
        {
            services.AddScoped<IAuditTrailRecorder, AuditTrailRecorder>();
        }
    }
}
