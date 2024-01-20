using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Infrastructure.Security.SwaggerSupport;

public class SwaggerJwtSupportConfiguration : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new()
        {
            In = ParameterLocation.Header,
            Description = "Please provide a valid jwt token.",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer",
        });

        options.AddSecurityRequirement(new()
        {
            {
                new()
                {
                    Reference = new()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        });
    }
}
