namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Queries.GetSubscription;

[Collection(WebAppFactoryCollection.CollectionName)]
public class GetSubscriptionTests(WebAppFactory webAppFactory)
{
    private readonly IMediator _mediator = webAppFactory.CreateMediator();

    [Fact]
    public async Task GetSubscription_WhenSubscriptionExists_ShouldReturnSubscription()
    {
        // Arrange
        var subscription = await _mediator.CreateSubscriptionAsync();

        var query = SubscriptionQueryFactory.CreateGetSubscriptionQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(subscription);
    }

    [Fact]
    public async Task GetSubscription_WhenNoSubscription_ShouldReturnNotFound()
    {
        // Arrange
        var query = SubscriptionQueryFactory.CreateGetSubscriptionQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
    }
}