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
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DismissReminder_WhenValidCommand_ShouldDismissReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var command = ReminderCommandFactory.CreateDismissReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsError.Should().BeFalse();
        getReminderResult.Value.IsDismissed.Should().BeTrue();
    }

    [Fact]
    public async Task DismissReminder_WhenReminderAlreadyDismissed_ShouldReturnConflict()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var command = ReminderCommandFactory.CreateDismissReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var firstDismissReminderResult = await _mediator.Send(command);
        var secondDismissReminderResult = await _mediator.Send(command);

        // Assert
        firstDismissReminderResult.IsError.Should().BeFalse();

        secondDismissReminderResult.IsError.Should().BeTrue();
        secondDismissReminderResult.FirstError.Type.Should().Be(ErrorType.Conflict);

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsError.Should().BeFalse();
        getReminderResult.Value.IsDismissed.Should().BeTrue();
    }
}