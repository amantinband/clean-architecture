using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Common;

using FluentEmail.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Infrastructure.Reminders.BackgroundServices;

public class ReminderEmailBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider _dateTimeProvider,
    IFluentEmail _fluentEmail) : IHostedService, IDisposable
{
    private readonly AppDbContext _dbContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    private Timer _timer = null!;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SendEmailNotifications, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    /// <summary>
    /// TODO: there are many edge cases that aren't caught here. This is an immediate nice to have implementation for now.
    /// </summary>
    private async void SendEmailNotifications(object? state)
    {
        var now = _dateTimeProvider.UtcNow;
        var oneMinuteFromNow = now.AddMinutes(1);

        var dueRemindersBySubscription = _dbContext.Reminders
            .Where(reminder => reminder.DateTime >= now && reminder.DateTime <= oneMinuteFromNow && !reminder.IsDismissed)
            .GroupBy(reminder => reminder.SubscriptionId)
            .ToList();

        var subscriptionToBeNotified = dueRemindersBySubscription.ConvertAll(x => x.Key);

        var usersToBeNotified = _dbContext.Users
            .Where(user => subscriptionToBeNotified.Contains(user.Subscription.Id))
            .ToList();

        foreach (User? user in usersToBeNotified)
        {
            var dueReminders = dueRemindersBySubscription
                .Single(x => x.Key == user.Subscription.Id)
                .ToList();

            await _fluentEmail
                .To(user.Email)
                .Subject($"{dueReminders.Count} reminders due!")
                .Body($"""
                      Dear {user.FirstName} {user.LastName} from the present.

                      I hope this email finds you well.

                      I'm writing you this email to remind you about the following reminders:
                      {string.Join('\n', dueReminders.Select((reminder, i) => $"{i}. {reminder.Text}"))}

                      Best,
                      {user.FirstName} from the past.
                      """)
                .SendAsync();
        }
    }
}