using CleanArchitecture.Api.Extensions.Constants;
using CleanArchitecture.Api.OperationFilters;
using Microsoft.OpenApi.Models;

namespace CleanArchitecture.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });

            options.AddSecurityDefinition(AuthSchemeConstants.ApiKeyAuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Please enter an API Key here",
            });

            //options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer",
            //            },
            //        },
            //        new string[] { }
            //    },
            //});

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        services.AddProblemDetails();

        return services;
    }
}