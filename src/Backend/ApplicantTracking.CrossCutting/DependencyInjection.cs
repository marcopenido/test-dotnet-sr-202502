using System.Threading.RateLimiting;
using ApplicantTracking.Application;
using ApplicantTracking.CrossCutting.Filters;
using ApplicantTracking.Infrastructure;
using ApplicantTracking.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;  
using Serilog;

namespace ApplicantTracking.CrossCutting;

public static class DependecyInjection
{
    public static void AddApiConfiguration(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
    }

    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                }));
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.RetryAfter = "60 seconds";
                await context.HttpContext.Response.WriteAsync("Too Many Requests", token);
            };
        });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        const string AUTHENTICATION_TYPE = "Bearer";

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = AUTHENTICATION_TYPE
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = AUTHENTICATION_TYPE
                        },
                        Scheme = "oauth2",
                        Name = AUTHENTICATION_TYPE,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        ApplicationDependecyInjection.AddAutoMapper(services);
        ApplicationDependecyInjection.AddUseCases(services);
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        InfrastructureDependecyInjection.AddDbContextSqlServer(services, configuration);
        InfrastructureDependecyInjection.AddRepositories(services);
        InfrastructureDependecyInjection.AddAuditTrail(services);

        if (configuration.IsUnitTestEnviroment())
            return;

        InfrastructureDependecyInjection.AddRedis(services, configuration);
    }

    public static void UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger, true);
    }

    public static void AddFilters(this IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });
    }

    public static void EnsureDatabaseMigrated(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        InfrastructureDependecyInjection.ApplyMigrations(app.ApplicationServices);
    }
}
