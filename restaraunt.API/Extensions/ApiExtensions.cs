using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using restaraunt.API.Controlers;
using restaraunt.Infrastructure;

namespace restaraunt.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey!))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["tasty"];
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireClaim("Admin", "true");
                });
            });
        }
    }
}