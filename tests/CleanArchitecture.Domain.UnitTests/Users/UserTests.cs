using CleanArchitecture.Domain.UnitTests.TestUtils.Reminders;
using CleanArchitecture.Domain.UnitTests.TestUtils.Users;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using FluentAssertions;

namespace CleanArchitecture.Domain.UnitTests.Users;

public class UserTests
{
    [Theory]
    [MemberData(nameof(PlanTypes))]
    public void SetReminder_WhenLessThanDailyRemindersLimit_ShouldSetReminder(PlanType planType)
    {
        // Arrange
        var calendar = CalendarFactory.Create(
            date: DateOnly.FromDateTime(ReminderConstants.DateTime.DateTime),
            numEvents: planType.GetMaxDailyReminders() - 1);

        var user = UserFactory.Create(planType: planType, calendar: calendar);

        var reminder = ReminderFactory.Create(dateTime: ReminderConstants.DateTime);

        // Act
        var setReminderResult = user.SetReminder(reminder);

        // Assert
        setReminderResult.IsError.Should().BeFalse();
        setReminderResult.Value.Should().Be(Result.Success);
    }

    [Theory]
    [MemberData(nameof(PlanTypes))]
    public void SetReminder_WhenReachedDailyRemindersLimit_ShouldNotSetReminder(PlanType planType)
    {
        // Arrange
        var calendar = CalendarFactory.Create(
            date: DateOnly.FromDateTime(ReminderConstants.DateTime.Date),
            numEvents: planType.GetMaxDailyReminders());

        var user = UserFactory.Create(planType: planType, calendar: calendar);

        var reminder = ReminderFactory.Create(dateTime: ReminderConstants.DateTime);

        // Act
        var setReminderResult = user.SetReminder(reminder);

        // Assert
        setReminderResult.IsError.Should().BeTrue();
        setReminderResult.FirstError.Should().Be(UserErrors.CannotCreateMoreRemindersThanPlanAllows);
    }

    public static TheoryData<PlanType> PlanTypes = [PlanType.Basic, PlanType.Pro];
}