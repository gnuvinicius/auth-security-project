using Swashbuckle.AspNetCore.SwaggerUI;

namespace Security.Configurations;

internal static class SwaggerUISetupOptions {

    internal static void Setup(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    }
}