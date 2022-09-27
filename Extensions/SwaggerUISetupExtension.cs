using Swashbuckle.AspNetCore.SwaggerUI;

namespace Security.Extensions;

internal static class SwaggerUISetupExtension
{
    internal static void ConfigureSwaggerUI(this WebApplication app)
    {
        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
}