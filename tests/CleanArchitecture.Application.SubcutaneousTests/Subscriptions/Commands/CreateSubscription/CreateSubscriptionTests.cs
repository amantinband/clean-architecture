namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

[Collection(WebAppFactoryCollection.CollectionName)]
public class CreateSubscriptionTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task CreateSubscription_WhenNoSubscription_ShouldCreateSubscription()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand().Value;

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.AssertCreatedFrom(command);

        var getSubscriptionResult = await _mediator.GetSubscriptionAsync();
        getSubscriptionResult.IsFailure.Should().BeFalse();
        getSubscriptionResult.Value.Should().BeEquivalentTo(result.Value);
    }

    [Fact]
    public async Task CreateSubscription_WhenSubscriptionAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand().Value;

        // Act
        var firstResult = await _mediator.Send(command);
        var secondResult = await _mediator.Send(command);

        // Assert
        firstResult.IsFailure.Should().BeFalse();
        firstResult.Value.AssertCreatedFrom(command);

        secondResult.IsFailure.Should().BeTrue();
        secondResult.Error.Should().BeOfType<ConflictError>();
    }
}