using CleanArchitecture.Infrastructure.Common.Middleware;

using Microsoft.AspNetCore.Builder;

namespace CleanArchitecture.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}