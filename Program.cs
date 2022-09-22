using Security.Models;
using Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Security.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// dependency injections
builder.Services.ResolveDependences();

builder.Services.AddDbContext<SecurityContext>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config => SwaggerGenSetupOptions.Setup(config));

#region security

var key = Encoding.ASCII.GetBytes(builder.Configuration["Secret"]);

builder.Services.AddAuthentication(options => AuthenticationSetupOptions.AuthenticationSetup(options))
    .AddJwtBearer(options =>  AuthenticationSetupOptions.JwtSetup(options, key));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("user", policy => policy.RequireClaim("Store", "User"));
    options.AddPolicy("admin", policy => policy.RequireClaim("Store", "Admin"));
});
#endregion

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});


#region configure app

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options => SwaggerUISetupOptions.Setup(options));

app.UseHsts();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

#endregion