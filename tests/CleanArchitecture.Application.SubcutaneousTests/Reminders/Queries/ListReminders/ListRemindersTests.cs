namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Queries.ListReminders;

[Collection(WebAppFactoryCollection.CollectionName)]
public class ListRemindersTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task ListReminders_WhenValidQuery_ShouldReturnReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();
        var reminder = await _mediator.SetReminderAsync(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var query = ReminderQueryFactory.CreateListRemindersQuery(
            subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().ContainSingle().Which.Should().BeEquivalentTo(reminder);
    }

    [Fact]
    public async Task ListReminders_WhenNoSubscription_ShouldReturnEmptyList()
    {
        // Arrange
        var query = ReminderQueryFactory.CreateListRemindersQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task ListReminders_WhenNoReminder_ShouldReturnEmptyList()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();

        var query = ReminderQueryFactory.CreateListRemindersQuery(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }
}