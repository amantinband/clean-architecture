namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionValidationTests
{
    private readonly IMediator _mediator;
    private readonly TestCurrentUserProvider _currentUserProvider;

    public CreateSubscriptionValidationTests()
    {
        var webAppFactory = new WebAppFactory();
        _mediator = webAppFactory.CreateMediator();
        _currentUserProvider = webAppFactory.TestCurrentUserProvider;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10001)]
    public async Task CreateSubscription_WhenInvalidFirstName_ShouldReturnValidationError(int nameLength)
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(
            firstName: new('a', nameLength));

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10001)]
    public async Task CreateSubscription_WhenInvalidLastName_ShouldReturnValidationError(int nameLength)
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(
            lastName: new('a', nameLength));

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Theory]
    [InlineData("foo.com")]
    [InlineData("foo")]
    public async Task CreateSubscription_WhenInvalidEmailAddress_ShouldReturnValidationError(string email)
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(email: email);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }
}