namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

[Collection(WebAppFactoryCollection.CollectionName)]
public class CreateSubscriptionTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task CreateSubscription_WhenNoSubscription_ShouldCreateSubscription()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.AssertCreatedFrom(command);

        var getSubscriptionResult = await _mediator.GetSubscriptionAsync();
        getSubscriptionResult.IsError.Should().BeFalse();
        getSubscriptionResult.Value.Should().BeEquivalentTo(result.Value);
    }

    [Fact]
    public async Task CreateSubscription_WhenSubscriptionAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var firstResult = await _mediator.Send(command);
        var secondResult = await _mediator.Send(command);

        // Assert
        firstResult.IsError.Should().BeFalse();
        firstResult.Value.AssertCreatedFrom(command);

        secondResult.IsError.Should().BeTrue();
        secondResult.FirstError.Type.Should().Be(ErrorType.Conflict);
    }
}