using Security.Models;
using Microsoft.EntityFrameworkCore;
using Security.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// dependency injections
builder.Services.ResolveDependences();

builder.Services.AddDbContext<SecurityContext>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwaggerGen();

builder.Services.AuthServices(builder);

#region configure app

var app = builder.Build();

app.UseSwagger();

app.ConfigureSwaggerUI();

app.UseHsts();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

#endregion