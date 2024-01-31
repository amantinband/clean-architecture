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
    public void CreateSubscription_WhenInvalidFirstName_ShouldReturnValidationError(int nameLength)
    {
        // Arrange & act
        var result = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(
            firstName: new('a', nameLength));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10001)]
    public void CreateSubscription_WhenInvalidLastName_ShouldReturnValidationError(int nameLength)
    {
        // Arrange & act
        var result = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(
            lastName: new('a', nameLength));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
    }

    [Theory]
    [InlineData("foo.com")]
    [InlineData("foo")]
    public void CreateSubscription_WhenInvalidEmailAddress_ShouldReturnValidationError(string email)
    {
        // Arrange & act
        var result = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(email: email);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ValidationError>();
    }
}