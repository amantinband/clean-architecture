using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Reminders.Persistence;
using CleanArchitecture.Infrastructure.Users.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuth()
            .AddPersistence();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = CleanArchitecture.db"));

        services.AddScoped<IRemindersRepository, RemindersRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();

        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        return services;
    }
}