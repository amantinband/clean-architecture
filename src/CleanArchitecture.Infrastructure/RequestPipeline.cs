using CleanArchitecture.Infrastructure.Common;

using Microsoft.AspNetCore.Builder;

namespace CleanArchitecture.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}