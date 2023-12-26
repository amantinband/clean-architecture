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
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Assert side effects took place
        var getSubscriptionResult = await _mediator.GetSubscriptionAsync();

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
        var subscription = await _mediator.CreateSubscriptionAsync();
        await _mediator.SetReminderAsync(ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));
        await _mediator.SetReminderAsync(ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Assert side effects took place
        var getSubscriptionResult = await _mediator.GetSubscriptionAsync();

        getSubscriptionResult.IsError.Should().BeTrue();
        getSubscriptionResult.FirstError.Type.Should().Be(ErrorType.NotFound);

        var listRemindersResult = await _mediator.ListRemindersAsync();

        listRemindersResult.IsError.Should().BeFalse();
        listRemindersResult.Value.Should().BeEmpty();
    }
}