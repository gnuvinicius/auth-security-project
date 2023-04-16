using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Security.Extensions;

internal static class AuthenticationSetupExtension
{
    internal static void AuthServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Secret"]);

        services.AddAuthentication(options => AuthenticationSetup(options))
            .AddJwtBearer(options => JwtSetup(options, key));

        services.AddAuthorization(options =>
        {
            options.AddPolicy("user", policy => policy.RequireClaim("Store", "User"));
            options.AddPolicy("admin", policy => policy.RequireClaim("Store", "Admin"));
        });

        services.AddMvc(config =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });
    }

    private static void AuthenticationSetup(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static void JwtSetup(JwtBearerOptions options, byte[] key)
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
}