using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using FluentAssertions;

using TestCommon.Reminders;
using TestCommon.Users;

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

        // Assert
        var allButLastSetReminderResults = setReminderResults[..^1];

        allButLastSetReminderResults.Should().AllSatisfy(
            setReminderResult => setReminderResult.Value.Should().Be(Result.Success));

        var lastReminder = setReminderResults.Last();

        lastReminder.IsError.Should().BeTrue();
        lastReminder.FirstError.Should().Be(UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows);
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