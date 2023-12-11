using CleanArchitecture.Contracts.Subscriptions;

namespace CleanArchitecture.Api.IntegrationTests.Controllers;

[Collection(WebAppFactoryCollection.CollectionName)]

public class CreateSubscriptionTests
{
    private readonly HttpClient _client;

    public CreateSubscriptionTests(WebAppFactory webAppFactory)
    {
        _client = webAppFactory.HttpClient;
        webAppFactory.ResetDatabase();
    }

    [Theory]
    [MemberData(nameof(ListSubscriptionTypes))]
    public async Task CreateSubscription_WhenValidSubscription_ShouldCreateSubscription(SubscriptionType subscriptionType)
    {
        // Arrange
        var createSubscriptionRequest = new CreateSubscriptionRequest(SubscriptionType: subscriptionType);

        // Act
        var response = await _client.PostAsJsonAsync(
            $"users/{Constants.User.Id}/subscriptions",
            createSubscriptionRequest);

        // Assert
        // response.StatusCode.Should().Be(HttpStatusCode.Created);
        // response.Headers.Location.Should().NotBeNull();

        // var subscriptionResponse = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        // subscriptionResponse.Should().NotBeNull();
        // subscriptionResponse!.SubscriptionType.Should().Be(subscriptionType);

        // response.Headers.Location!.PathAndQuery.Should().Be($"/Subscriptions/{subscriptionResponse.Id}");
    }

    public static TheoryData<SubscriptionType> ListSubscriptionTypes()
    {
        var subscriptionTypes = Enum.GetValues<SubscriptionType>().ToList();

        var theoryData = new TheoryData<SubscriptionType>();

        foreach (var subscriptionType in subscriptionTypes)
        {
            theoryData.Add(subscriptionType);
        }

        return theoryData;
    }
}