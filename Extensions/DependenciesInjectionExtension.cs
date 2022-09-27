using Security.Services;

namespace Security.Extensions;

internal static class DependenciesInjectionExtension
{
    internal static IServiceCollection ResolveDependences(this IServiceCollection services) {

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}