using restaraunt.Application.Interfaces.Auth;
using restaraunt.Application.Interfaces.Repositories;
using restaraunt.Application.Services;
using restaraunt.Persistence;
using restaraunt.Core.Entities;
using restaraunt.Persistence.Repositories;
using restaraunt.Infrastructure;
using restaraunt.API.Controlers;
using restaraunt.API.Extensions;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddDbContext<AppDbContext>();
services.AddScoped<IUsersRepository, UsersRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();

services.AddAuthentication();
services.AddScoped<UserService>();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddApiAuthentication(configuration);
services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapUsersEndpoints();
app.MapControllers();
app.MapGet("/", () => "Hello World!")
    .RequireAuthorization("AdminPolicy");

app.Run();
