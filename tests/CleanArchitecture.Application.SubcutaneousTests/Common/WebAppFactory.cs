using CleanArchitecture.Api;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Application.SubcutaneousTests.Common;

public class WebAppFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    public TestCurrentUserProvider TestCurrentUserProvider { get; private set; } = new();
    public SqliteTestDatabase TestDatabase { get; set; } = null!;

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();

        TestDatabase.ResetDatabase();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public new Task DisposeAsync()
    {
        TestDatabase.Dispose();

        return Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        TestDatabase = SqliteTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<ICurrentUserProvider>()
                .AddScoped<ICurrentUserProvider>(_ => TestCurrentUserProvider);

            services
                .RemoveAll<DbContextOptions<AppDbContext>>()
                .AddDbContext<AppDbContext>((sp, options) => options.UseSqlite(TestDatabase.Connection));
        });
    }
}