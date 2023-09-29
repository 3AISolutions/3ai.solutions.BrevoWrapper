using Microsoft.Extensions.DependencyInjection;
using sib_api_v3_sdk.Client;

namespace _3ai.solutions.BrevoWrapper;

public static class DependencyInjection
{
    public static IServiceCollection AddBrevoClient(this IServiceCollection services, string apiKey)
    {
        services.AddSingleton<BrevoClient>();
        Configuration.Default.ApiKey.Add("api-key", apiKey);
        return services;
    }
}
