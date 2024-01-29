using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionAuthorizationTests
{
    private readonly IMediator _mediator;
    private readonly TestCurrentUserProvider _currentUserProvider;

    public GetSubscriptionAuthorizationTests()
    {
        var webAppFactory = new WebAppFactory();
        _mediator = webAppFactory.CreateMediator();
        _currentUserProvider = webAppFactory.TestCurrentUserProvider;
    }

    [Fact]
    public async Task GetSubscriptionForDifferentUser_WhenIsAdmin_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.Error.Should().NotBeOfType<ForbiddenError>();
    }

    [Fact]
    public async Task GetSubscriptionForDifferentUser_WhenIsNotAdmin_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.Error.Should().BeOfType<ForbiddenError>();
    }

    [Fact]
    public async Task GetSubscriptionForSelf_WhenDoesNotHaveRequiredPermissions_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            permissions: [],
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.Error.Should().BeOfType<ForbiddenError>();
    }

    [Fact]
    public async Task GetSubscriptionForSelf_WhenHasRequiredPermissions_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            permissions: [Permission.Subscription.Get],
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.Error.Should().NotBeOfType<ForbiddenError>();
    }
}