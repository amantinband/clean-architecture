namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CancelSubscription;

[Collection(WebAppFactoryCollection.CollectionName)]
public class CancelSubscriptionTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task CancelSubscription_WhenSubscriptionExists_ShouldCancelSubscription()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(default(FunctionalDdd.Unit));

        // Assert side effects took place
        var getSubscriptionResult = await _mediator.GetSubscriptionAsync();

        getSubscriptionResult.IsFailure.Should().BeTrue();
        getSubscriptionResult.Error.Should().BeOfType<NotFoundError>();
    }

    [Fact]
    public async Task CancelSubscription_WhenSubscriptionDoesNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
    }

    [Fact]
    public async Task CancelSubscription_WhenSubscriptionHasReminders_ShouldCancelSubscriptionAndDeleteReminders()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        await _mediator.SetReminderAsync(ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);
        await _mediator.SetReminderAsync(ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(default(FunctionalDdd.Unit));

        // Assert side effects took place
        var getSubscriptionResult = await _mediator.GetSubscriptionAsync();

        getSubscriptionResult.IsFailure.Should().BeTrue();
        getSubscriptionResult.Error.Should().BeOfType<NotFoundError>();

        var listRemindersResult = await _mediator.ListRemindersAsync();

        listRemindersResult.IsFailure.Should().BeFalse();
        listRemindersResult.Value.Should().BeEmpty();
    }
}