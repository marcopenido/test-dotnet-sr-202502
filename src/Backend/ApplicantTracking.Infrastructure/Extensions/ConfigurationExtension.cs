using Microsoft.Extensions.Configuration;

namespace ApplicantTracking.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnviroment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("ApplicantTrackingDb")!;
    }

    public static string RedisConnectionString(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Settings:Redis:ConnectionString")!;
    }
}
