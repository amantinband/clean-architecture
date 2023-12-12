using System.Net.Http.Headers;

using CleanArchitecture.Api.IntegrationTests.Common.Subscriptions;
using CleanArchitecture.Api.IntegrationTests.Common.Tokens;
using CleanArchitecture.Contracts.Subscriptions;
using CleanArchitecture.Contracts.Tokens;

namespace CleanArchitecture.Api.IntegrationTests.Common;

public class AppHttpClient(HttpClient _httpClient)
{
    public async Task<SubscriptionResponse> CreateSubscriptionAndExpectSuccessAsync(
        Guid? userId = null,
        CreateSubscriptionRequest? createSubscriptionRequest = null,
        string? token = null)
    {
        var response = await CreateSubscriptionAsync(userId, createSubscriptionRequest, token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var subscriptionResponse = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();

        subscriptionResponse.Should().NotBeNull();

        return subscriptionResponse!;
    }

    public async Task<string> GenerateTokenAsync(
        GenerateTokenRequest? generateTokenRequest = null)
    {
        generateTokenRequest ??= TokenRequestFactory.CreateGenerateTokenRequest();

        var response = await _httpClient.PostAsJsonAsync("tokens/generate", generateTokenRequest);

        response.Should().BeSuccessful();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        tokenResponse.Should().NotBeNull();

        return tokenResponse!.Token;
    }

    public async Task<HttpResponseMessage> CreateSubscriptionAsync(
        Guid? userId = null,
        CreateSubscriptionRequest? createSubscriptionRequest = null,
        string? token = null)
    {
        userId ??= Constants.User.Id;
        createSubscriptionRequest ??= SubscriptionRequestFactory.CreateCreateSubscriptionRequest();
        token ??= await GenerateTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await _httpClient.PostAsJsonAsync($"users/{userId}/subscriptions", createSubscriptionRequest);
    }
}