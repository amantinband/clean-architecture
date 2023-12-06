using CleanArchitecture.Domain.UnitTests.TestUtils.Reminders;
using CleanArchitecture.Domain.UnitTests.TestUtils.Users;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using FluentAssertions;

namespace CleanArchitecture.Domain.UnitTests.Users;

public class UserTests
{
    [Theory]
    [MemberData(nameof(ListSubscriptionTypes))]
    public void SetReminder_WhenLessThanDailyRemindersLimit_ShouldSetReminder(SubscriptionType SubscriptionType)
    {
        // Arrange
        var calendar = CalendarFactory.Create(
            date: DateOnly.FromDateTime(ReminderConstants.DateTime.DateTime),
            numEvents: SubscriptionType.GetMaxDailyReminders() - 1);

        var user = UserFactory.Create(SubscriptionType: SubscriptionType, calendar: calendar);

        var reminder = ReminderFactory.Create(dateTime: ReminderConstants.DateTime);

        // Act
        var setReminderResult = user.SetReminder(reminder);

        // Assert
        setReminderResult.IsError.Should().BeFalse();
        setReminderResult.Value.Should().Be(Result.Success);
    }

    [Theory]
    [MemberData(nameof(ListSubscriptionTypes))]
    public void SetReminder_WhenReachedDailyRemindersLimit_ShouldNotSetReminder(SubscriptionType SubscriptionType)
    {
        // Arrange
        var calendar = CalendarFactory.Create(
            date: DateOnly.FromDateTime(ReminderConstants.DateTime.Date),
            numEvents: SubscriptionType.GetMaxDailyReminders());

        var user = UserFactory.Create(SubscriptionType: SubscriptionType, calendar: calendar);

        var reminder = ReminderFactory.Create(dateTime: ReminderConstants.DateTime);

        // Act
        var setReminderResult = user.SetReminder(reminder);

        // Assert
        setReminderResult.IsError.Should().BeTrue();
        setReminderResult.FirstError.Should().Be(UserErrors.CannotCreateMoreRemindersThanPlanAllows);
    }

    public static TheoryData<SubscriptionType> ListSubscriptionTypes()
    {
        TheoryData<SubscriptionType> theoryData = [];

        foreach (var subscriptionType in SubscriptionType.List)
        {
            theoryData.Add(subscriptionType);
        }

        return theoryData;
    }
}