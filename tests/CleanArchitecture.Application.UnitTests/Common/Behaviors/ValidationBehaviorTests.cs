using CleanArchitecture.Application.Common.Behaviors;
using CleanArchitecture.Application.Reminders.Commands.SetReminder;
using CleanArchitecture.Domain.Reminders;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace CleanArchitecture.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly ValidationBehavior<SetReminderCommand, Result<Reminder>> _validationBehavior;
    private readonly IValidator<SetReminderCommand> _mockValidator;
    private readonly RequestHandlerDelegate<Result<Reminder>> _mockNextBehavior;

    public ValidationBehaviorTests()
    {
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<Result<Reminder>>>();
        _mockValidator = Substitute.For<IValidator<SetReminderCommand>>();

        _validationBehavior = new(_mockValidator);
    }

    [Fact]
    public async Task InvokeValidationBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var setReminderCommand = ReminderCommandFactory.CreateSetReminderCommand();
        var reminder = ReminderFactory.CreateReminder();

        _mockValidator
            .ValidateAsync(setReminderCommand, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _mockNextBehavior.Invoke().Returns(reminder);

        // Act
        var result = await _validationBehavior.Handle(setReminderCommand, _mockNextBehavior, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(reminder);
    }

    [Fact]
    public async Task InvokeValidationBehavior_WhenValidatorResultIsNotValid_ShouldReturnListOfErrors()
    {
        // Arrange
        var setReminderCommand = ReminderCommandFactory.CreateSetReminderCommand();
        List<ValidationFailure> validationFailures = [new(propertyName: "foo", errorMessage: "bad foo")];

        _mockValidator
            .ValidateAsync(setReminderCommand, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _validationBehavior.Handle(setReminderCommand, _mockNextBehavior, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
        var validationError = (ValidationError)result.Error;
        validationError.Errors.Should().HaveCount(1);
        validationError.Errors[0].FieldName.Should().Be("foo");
        validationError.Errors[0].Message.Should().Be("bad foo");
    }

    [Fact]
    public async Task InvokeValidationBehavior_WhenNoValidator_ShouldInvokeNextBehavior()
    {
        // Arrange
        var setReminderCommand = ReminderCommandFactory.CreateSetReminderCommand();
        var validationBehavior = new ValidationBehavior<SetReminderCommand, Result<Reminder>>();

        var reminder = ReminderFactory.CreateReminder();
        _mockNextBehavior.Invoke().Returns(reminder);

        // Act
        var result = await validationBehavior.Handle(setReminderCommand, _mockNextBehavior, default);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(reminder);
    }
}