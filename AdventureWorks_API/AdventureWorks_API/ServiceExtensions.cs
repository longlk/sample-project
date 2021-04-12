using AdventureWorks_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace AdventureWorks_API
{
    public static class ServiceExtensions
    {
        public static void ConnectDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AdventureWorksContext>
                (options =>
                                options.UseSqlServer(configuration.GetConnectionString("AdventureWorksDatabase"))
                );
        }

        public static void EnableSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Chuong's API" });
            });
        }
        public static void EnableCors(this IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        public static void ConfigureJWtBearer(this IServiceCollection services)
        {
            var secretKey = Encoding.ASCII.GetBytes("9135176d-94830-456b-aff8-3dsf7b85b05f26");
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = true,
                    ValidIssuer = "https://quizdeveloper.com",
                    ValidateAudience = true,
                    ValidAudience = "https://quizdeveloper.com",
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
