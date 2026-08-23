using CleanArchitecture.Api.Extensions.Constants;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Api.OperationFilters;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 1. Nếu API có [AllowAnonymous] -> Ẩn hoàn toàn cái khóa
        var hasAllowAnonymous = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AllowAnonymousAttribute>()
            .Any();

        if (hasAllowAnonymous)
        {
            operation.Security.Clear();
            return;
        }

        // 2. Lấy các [Authorize] attribute trên Controller/Action
        var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>()
            .ToList();

        if (authAttributes.Any())
        {
            operation.Security = new List<OpenApiSecurityRequirement>();

            foreach (var attr in authAttributes)
            {
                // Nếu [Authorize(AuthenticationSchemes = AuthSchemeConstants.ApiKeyAuthenticationScheme)] (hoặc AuthSchemeConstants.ApiKeyAuthenticationScheme)
                if (!string.IsNullOrEmpty(attr.AuthenticationSchemes) &&
                    attr.AuthenticationSchemes.Equals(AuthSchemeConstants.ApiKeyAuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                {
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = AuthSchemeConstants.ApiKeyAuthenticationScheme
                                }
                            },
                            new string[] { }
                        },
                    });
                }
                else // Mặc định [Authorize] sẽ dùng Bearer
                {
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                }
            }
        }
    }
}