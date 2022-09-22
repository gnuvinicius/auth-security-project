using Security.Services;

namespace Security.Configurations;

internal static class DependenciesInjectionSetup {

    internal static IServiceCollection ResolveDependences(this IServiceCollection services) {

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}