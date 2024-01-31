namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Commands.DismissReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class DismissReminderTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task DismissReminder_WhenSubscriptionDoesNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var command = ReminderCommandFactory.CreateDismissReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
    }

    [Fact]
    public async Task DismissReminder_WhenValidCommand_ShouldDismissReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);

        var command = ReminderCommandFactory.CreateDismissReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(default(FunctionalDdd.Unit));

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsFailure.Should().BeFalse();
        getReminderResult.Value.IsDismissed.Should().BeTrue();
    }

    [Fact]
    public async Task DismissReminder_WhenReminderAlreadyDismissed_ShouldReturnConflict()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);

        var command = ReminderCommandFactory.CreateDismissReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var firstDismissReminderResult = await _mediator.Send(command);
        var secondDismissReminderResult = await _mediator.Send(command);

        // Assert
        firstDismissReminderResult.IsSuccess.Should().BeTrue();

        secondDismissReminderResult.IsFailure.Should().BeTrue();
        secondDismissReminderResult.Error.Should().BeOfType<ConflictError>();

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsFailure.Should().BeFalse();
        getReminderResult.Value.IsDismissed.Should().BeTrue();
    }
}