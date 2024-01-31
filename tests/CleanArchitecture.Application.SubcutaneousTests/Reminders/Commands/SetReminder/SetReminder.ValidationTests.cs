namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Commands.SetReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class SetReminderValidationTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public void SetReminder_WhenInvalidDateTime_ShouldReturnValidationError()
    {
        // Arrange & Act
        var result = ReminderCommandFactory.CreateSetReminderCommand(dateTime: DateTime.UtcNow.AddDays(-1));

        // Act
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10001)]
    public void SetReminder_WhenInvalidText_ShouldReturnValidationError(int textLength)
    {
        // Arrange & act
        var result = ReminderCommandFactory.CreateSetReminderCommand(text: new string('a', textLength));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
    }
}