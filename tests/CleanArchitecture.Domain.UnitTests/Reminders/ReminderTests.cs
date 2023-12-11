namespace CleanArchitecture.Domain.UnitTests.Reminders;

public class ReminderTests
{
    [Fact]
    public void CreateReminder_WhenConstructedSuccessfully_ShouldHaveIsDismissedFalse()
    {
        // Act
        var reminder = ReminderFactory.CreateReminder();

        // Assert
        reminder.IsDismissed.Should().BeFalse();
    }

    [Fact]
    public void DismissReminder_WhenReminderNotDismissed_ShouldDismissReminder()
    {
        // Arrange
        var reminder = ReminderFactory.CreateReminder();

        // Act
        var dismissReminderResult = reminder.Dismiss();

        // Assert
        dismissReminderResult.IsError.Should().BeFalse();
        reminder.IsDismissed.Should().BeTrue();
    }

    [Fact]
    public void DismissReminder_WhenReminderAlreadyDismissed_ShouldReturnConflict()
    {
        // Arrange
        var reminder = ReminderFactory.CreateReminder();

        // Act
        var firstDismissReminderResult = reminder.Dismiss();
        var secondDismissReminderResult = reminder.Dismiss();

        // Assert
        firstDismissReminderResult.IsError.Should().BeFalse();

        secondDismissReminderResult.IsError.Should().BeTrue();
        secondDismissReminderResult.FirstError.Type.Should().Be(ErrorType.Conflict);
    }
}