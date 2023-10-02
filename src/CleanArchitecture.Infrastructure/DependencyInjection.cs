using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Reminders.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = CleanArchitecture.db"));

        services.AddScoped<IRemindersRepository, RemindersRepository>();

        return services;
    }
}