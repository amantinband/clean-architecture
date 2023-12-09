using CleanArchitecture.Api;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;

using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TestCommon.Security;

namespace CleanArchitecture.Application.SubcutaneousTests.Common;

public class MediatorFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    public TestCurrentUserProvider TestCurrentUserProvider { get; set; } = new();

    private SqliteTestDatabase _testDatabase = null!;

    public IMediator CreateMediator()
    {
        var serviceScope = Services.CreateScope();

        _testDatabase.ResetDatabase();

        return serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public new Task DisposeAsync()
    {
        _testDatabase.Dispose();

        return Task.CompletedTask;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqliteTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<ICurrentUserProvider>()
                .AddSingleton<ICurrentUserProvider>(TestCurrentUserProvider);

            services
                .RemoveAll<DbContextOptions<AppDbContext>>()
                .AddDbContext<AppDbContext>((sp, options) => options.UseSqlite(_testDatabase.Connection));
        });
    }
}