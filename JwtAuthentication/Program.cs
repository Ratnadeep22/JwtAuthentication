using JwtAuthentication;
using JwtAuthentication.APIFilters;
using JwtAuthentication.Utilities;
using Microsoft.OpenApi.Models;
using ModelObjects;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var JwtInfo = new JwtInfo();
        builder.Configuration.GetSection("JwtInfo").Bind(JwtInfo);
        builder.Services.AddSingleton(JwtInfo);
        builder.Services.configureJWTServices(builder.Configuration);

        builder.Services.AddScoped<TokenModel, TokenModel>();
        builder.Services.AddTransient<ITokenManager, TokenManager>();

        builder.Services.AddControllers(mvcoptions =>
        {
            mvcoptions.Filters.Add(typeof(MapperAttribute));
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(swaggergenoptions =>
        {
            swaggergenoptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authentication e.g. Bearer abcd",
                Scheme = "Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
            });

            swaggergenoptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { 
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference{ 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}