namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Commands.DeleteReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class DeleteReminderTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task DeleteReminder_WhenSubscriptionDoesNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var command = ReminderCommandFactory.CreateDeleteReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteReminder_WhenReminderDoesNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var command = ReminderCommandFactory.CreateDeleteReminderCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteReminder_WhenValidCommand_ShouldDeleteReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var command = ReminderCommandFactory.CreateDeleteReminderCommand(
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

        getReminderResult.IsError.Should().BeTrue();
        getReminderResult.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteReminder_WhenReminderAlreadyDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var command = ReminderCommandFactory.CreateDeleteReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var firstDeleteReminderResult = await _mediator.Send(command);
        var secondDeleteReminderResult = await _mediator.Send(command);

        // Assert
        firstDeleteReminderResult.IsError.Should().BeFalse();

        secondDeleteReminderResult.IsError.Should().BeTrue();
        secondDeleteReminderResult.FirstError.Type.Should().Be(ErrorType.NotFound);

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsError.Should().BeTrue();
        getReminderResult.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}