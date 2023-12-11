namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Queries.GetReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class GetReminderTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task GetReminder_WhenValidQuery_ShouldReturnReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscription();
        var reminder = await _mediator.SetReminder(
            ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id));

        var query = ReminderQueryFactory.CreateGetReminderQuery(
            subscriptionId: subscription.Id,
            reminderId: reminder.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsError.Should().BeFalse();
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
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Reminder");
    }

    [Fact]
    public async Task GetReminder_WhenNoReminder_ShouldReturnNotFound()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscription();

        var query = ReminderQueryFactory.CreateGetReminderQuery(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Reminder");
    }
}