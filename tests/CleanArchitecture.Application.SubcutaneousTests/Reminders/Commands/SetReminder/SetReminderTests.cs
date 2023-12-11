using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Commands.SetReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class SetReminderTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task SetReminder_WhenSubscriptionDoesNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var command = ReminderCommandFactory.CreateSetReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task SetReminder_WhenValidCommand_ShouldSetReminder()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscription();

        var command = ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.AssertCreatedFrom(command);

        // Assert side effects took place
        var getReminderResult = await _mediator.GetReminder(
            ReminderQueryFactory.CreateGetReminderQuery(
                subscriptionId: subscription.Id,
                reminderId: result.Value.Id));

        getReminderResult.IsError.Should().BeFalse();
        getReminderResult.Value.Should().BeEquivalentTo(result.Value);
    }

    [Theory]
    [MemberData(nameof(ListSubscriptionsWithLimit))]
    public async Task SetReminder_WhenMoreThanMaxDailyReminders_ShouldReturnValidationError(SubscriptionType subscriptionType)
    {
        // Arrange
        var subscription = await _mediator.CreateSubscription(
            SubscriptionCommandFactory.CreateCreateSubscriptionCommand(subscriptionType: subscriptionType));

        var commands = Enumerable.Range(0, subscriptionType.GetMaxDailyReminders() + 1)
            .Select(_ => ReminderCommandFactory.CreateSetReminderCommand(subscriptionId: subscription.Id))
            .ToList();

        // Act
        var results = await Task.WhenAll(commands.Select(command => _mediator.Send(command)));

        // Assert all but one created successfully
        var succeededCommands = results.Where(result => !result.IsError).ToList();
        succeededCommands.Should().HaveCount(subscriptionType.GetMaxDailyReminders());

        // Assert one returned the domain error we expect
        var failedCommands = results.Where(result => result.IsError).ToList();
        failedCommands.Should().ContainSingle()
            .Which.FirstError.Should().Be(UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows);

        // Assert side effects took place
        var listRemindersResult = await _mediator.ListReminders(
            ReminderQueryFactory.CreateListRemindersQuery(subscriptionId: subscription.Id));

        listRemindersResult.IsError.Should().BeFalse();
        listRemindersResult.Value.Should().HaveCount(subscriptionType.GetMaxDailyReminders());

        foreach (var succeededCommand in succeededCommands)
        {
            var result = await _mediator.GetReminder(
                ReminderQueryFactory.CreateGetReminderQuery(
                    subscriptionId: subscription.Id,
                    reminderId: succeededCommand.Value.Id));

            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(succeededCommand.Value);
        }
    }

    /// <summary>
    /// This is completely redundant as there is only one subscription with a limit.
    /// I added this here just so you have a copy-paste method for your own usage.
    /// </summary>
    public static TheoryData<SubscriptionType> ListSubscriptionsWithLimit()
    {
        TheoryData<SubscriptionType> theoryData = [];

        SubscriptionType.List.Except([SubscriptionType.Pro]).ToList()
            .ForEach(theoryData.Add);

        return theoryData;
    }
}