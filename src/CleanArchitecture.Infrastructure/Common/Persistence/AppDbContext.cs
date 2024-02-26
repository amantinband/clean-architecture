using CleanArchitecture.Application;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Common.Middleware;

using FunctionalDdd;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Common;

public class AppDbContext(DbContextOptions options, IHttpContextAccessor _httpContextAccessor, IPublisher _publisher) : DbContext(options)
{
    public DbSet<Reminder> Reminders { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<IAggregate>()
           .SelectMany(entry => entry.Entity.UncommittedEvents())
           .ToList();

        foreach (var entry in ChangeTracker.Entries<IAggregate>())
        {
            entry.Entity.AcceptChanges();
        }

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
            return await base.SaveChangesAsync(cancellationToken);
        }

        await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder
        .Entity<Reminder>()
        .Property(e => e.Id)
        .HasConversion(
            v => v.ToString(),
            v => ReminderId.TryCreate(v).Value);

        modelBuilder
        .Entity<Reminder>()
        .Property(e => e.UserId)
        .HasConversion(
            v => v.ToString(),
            v => UserId.TryCreate(v).Value);

        modelBuilder
        .Entity<User>()
        .Property(e => e.Id)
        .HasConversion(
            v => v.ToString(),
            v => UserId.TryCreate(v).Value);

        base.OnModelCreating(modelBuilder);
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var @event = GetNotificationEvent(domainEvent);
            await _publisher.Publish(@event);
        }
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        Queue<IDomainEvent> domainEventsQueue = _httpContextAccessor.HttpContext!.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value) &&
            value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;
    }

    private INotification GetNotificationEvent(IDomainEvent @event)
    {
        var eventType = @event.GetType();

        var notification =
            Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(eventType), @event) as
                INotification;

        return notification!;
    }
}