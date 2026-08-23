using System.Net;
using System.Net.Mail;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Repositories;
using CleanArchitecture.Infrastructure.Common.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Redis;
using CleanArchitecture.Infrastructure.Reminders.BackgroundServices;
using CleanArchitecture.Infrastructure.Reminders.Persistence;
using CleanArchitecture.Infrastructure.Security;
using CleanArchitecture.Infrastructure.Security.ApiKeyValidation;
using CleanArchitecture.Infrastructure.Security.CurrentUserProvider;
using CleanArchitecture.Infrastructure.Security.PolicyEnforcer;
using CleanArchitecture.Infrastructure.Security.TokenGenerator;
using CleanArchitecture.Infrastructure.Security.TokenValidation;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Infrastructure.Users.Persistence;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddServices()
            .AddBackgroundServices(configuration)
            .AddAuthentication(configuration)
            .AddAuthorization()
            .AddPersistence();

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEmailNotifications(configuration);

        return services;
    }

    private static IServiceCollection AddEmailNotifications(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        EmailSettings emailSettings = new();
        configuration.Bind(EmailSettings.Section, emailSettings);

        if (!emailSettings.EnableEmailNotifications)
        {
            return services;
        }

        services.AddHostedService<ReminderEmailBackgroundService>();

        services
            .AddFluentEmail(emailSettings.DefaultFromEmail)
            .AddSmtpSender(new SmtpClient(emailSettings.SmtpSettings.Server)
            {
                Port = emailSettings.SmtpSettings.Port,
                Credentials = new NetworkCredential(
                    emailSettings.SmtpSettings.Username,
                    emailSettings.SmtpSettings.Password),
            });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source = CleanArchitecture.sqlite"));

        services.AddScoped<IRemindersRepository, RemindersRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddSingleton<IPolicyEnforcer, PolicyEnforcer>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

        // Jwt token generator depends on scoped repositories, so register as scoped
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenRepository, RedisRefreshTokenRepository>();

        services
            .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("apiKey", options => { });

        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect("localhost:6379"));

        services.AddScoped<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

        return services;
    }
}