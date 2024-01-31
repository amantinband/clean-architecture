namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Queries.GetReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class GetReminderTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task GetReminder_WhenValidQuery_ShouldReturnReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id).Value);

        var query = ReminderQueryFactory.CreateGetReminderQuery(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(reminder);
    }

    [Fact]
    public async Task GetReminder_WhenNoSubscription_ShouldReturnNotFound()
    {
        // Arrange
        var query = ReminderQueryFactory.CreateGetReminderQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.NotFound("Reminder"));
    }

    [Fact]
    public async Task GetReminder_WhenNoReminder_ShouldReturnNotFound()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();

        var query = ReminderQueryFactory.CreateGetReminderQuery(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.NotFound("Reminder"));
    }
}