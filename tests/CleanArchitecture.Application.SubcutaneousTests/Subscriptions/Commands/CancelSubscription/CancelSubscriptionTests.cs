using CleanArchitecture.Application.SubcutaneousTests.Common;

using ErrorOr;

using FluentAssertions;

using MediatR;

using TestCommon.Reminders;
using TestCommon.Subscriptions;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionTests(MediatorFactory mediatorFactory)
    : IClassFixture<MediatorFactory>
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task CancelSubscription_WhenSubscriptionExists_ShouldCancelSubscription()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscription();

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Assert side effects took place
        var getSubscriptionResult = await _mediator.GetSubscription();

        getSubscriptionResult.IsError.Should().BeTrue();
        getSubscriptionResult.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CancelSubscription_WhenSubscriptionDoesNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task CancelSubscription_WhenSubscriptionHasReminders_ShouldCancelSubscriptionAndDeleteReminders()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscription();
        await _mediator.SetReminder(ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));
        await _mediator.SetReminder(ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Assert side effects took place
        var getSubscriptionResult = await _mediator.GetSubscription();

        getSubscriptionResult.IsError.Should().BeTrue();
        getSubscriptionResult.FirstError.Type.Should().Be(ErrorType.NotFound);

        var listRemindersResult = await _mediator.ListReminders();

        listRemindersResult.IsError.Should().BeFalse();
        listRemindersResult.Value.Should().BeEmpty();
    }
}