namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Commands.SetReminder;

[Collection(WebAppFactoryCollection.CollectionName)]
public class SetReminderValidationTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task SetReminder_WhenInvalidDateTime_ShouldReturnValidationError()
    {
        // Arrange
        var command = ReminderCommandFactory.CreateSetReminderCommand(dateTime: DateTime.UtcNow.AddDays(-1));

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10001)]
    public async Task SetReminder_WhenInvalidText_ShouldReturnValidationError(int textLength)
    {
        // Arrange
        var command = ReminderCommandFactory.CreateSetReminderCommand(text: new string('a', textLength));

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }
}