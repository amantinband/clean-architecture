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
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<AggregateError>();
        var aggregateError = (AggregateError)result.Error;
        aggregateError.Errors.Should().HaveCount(2);
        aggregateError.Errors[0].Should().BeOfType<NotFoundError>();
        aggregateError.Errors[0].Message.Should().Be("Reminder not found");
        aggregateError.Errors[1].Should().BeOfType<NotFoundError>();
        aggregateError.Errors[1].Message.Should().Be("User not found");
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
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
    }

    [Fact]
    public async Task DeleteReminder_WhenValidCommand_ShouldDeleteReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);

        var command = ReminderCommandFactory.CreateDeleteReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(default(FunctionalDdd.Unit));

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsFailure.Should().BeTrue();
        getReminderResult.Error.Should().BeOfType<NotFoundError>();
    }

    [Fact]
    public async Task DeleteReminder_WhenReminderAlreadyDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);

        var command = ReminderCommandFactory.CreateDeleteReminderCommand(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var firstDeleteReminderResult = await _mediator.Send(command);
        var secondDeleteReminderResult = await _mediator.Send(command);

        // Assert
        firstDeleteReminderResult.IsFailure.Should().BeFalse();

        secondDeleteReminderResult.IsFailure.Should().BeTrue();
        secondDeleteReminderResult.Error.Should().BeOfType<NotFoundError>();

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminderAsync(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: reminder.Id));

        getReminderResult.IsFailure.Should().BeTrue();
        getReminderResult.Error.Should().BeOfType<NotFoundError>();
    }
}