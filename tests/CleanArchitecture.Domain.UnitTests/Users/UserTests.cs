using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Domain.UnitTests.Users;

public class UserTests
{
    [Theory]
    [MemberData(nameof(ListSubscriptionsWithLimit))]
    public void SetReminder_WhenMoreThanSubscriptionAllows_ShouldFail(SubscriptionType subscriptionType)
    {
        // Arrange
        // Create user
        var subscription = SubscriptionFactory.CreateSubscription(subscriptionType: subscriptionType);
        var user = UserFactory.CreateUser(subscription: subscription);

        // Create max number of daily reminders + 1
        var reminders = Enumerable.Range(0, subscriptionType.GetMaxDailyReminders() + 1)
            .Select(_ => ReminderFactory.CreateReminder(id: Guid.NewGuid(), subscriptionId: subscription.Id));

        // Act
        var setReminderResults = reminders.Select(user.SetReminder).ToList();

        // Assert all reminders set successfully
        var allButLastSetReminderResults = setReminderResults[..^1];

        allButLastSetReminderResults.Should().AllSatisfy(
            setReminderResult => setReminderResult.Value.Should().Be(default(Unit)));

        // Assert settings last reminder returned conflict
        var lastReminder = setReminderResults.Last();

        lastReminder.IsFailure.Should().BeTrue();
        lastReminder.Error.Should().Be(UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows);
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