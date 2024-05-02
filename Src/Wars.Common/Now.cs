using Microsoft.Extensions.DependencyInjection;

namespace Wars.Common;

public delegate DateTimeOffset Now();

public static class NowServiceExtensions
{
    public static IServiceCollection AddNow(this IServiceCollection services) =>
        services.AddSingleton<Now>(() => DateTimeOffset.UtcNow);
}
