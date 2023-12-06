using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthentication
{
    public static class ConfigurationExtension
    {
        public static void configureJWTServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(ao=>
            {
                ao.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ao.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                ao.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(beareroptions =>
                {
                    beareroptions.TokenValidationParameters = new TokenValidationParameters {
                        ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                        ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SecretKey")!)),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
                });
        }
    }
}
