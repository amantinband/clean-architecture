using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Application.SubcutaneousTests.Common;

using FluentAssertions;

using MediatR;

using TestCommon.Security;
using TestCommon.Subscriptions;
using TestCommon.TestConstants;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Queries.GetSubscription;

public class CreateSubscriptionAuthorizationTests(MediatorFactory mediatorFactory)
    : IClassFixture<MediatorFactory>
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    private readonly TestCurrentUserProvider _currentUserProvider = mediatorFactory.TestCurrentUserProvider;

    [Fact]
    public async Task GetSubscription_WhenDifferentUserButWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery(
            userId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task GetSubscription_WhenDifferentUserButWithoutAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery(
            userId: Guid.NewGuid());

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task GetSubscription_WhenGettingForSelfButWithoutRequiredPermissions_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            permissions: [],
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery(
            userId: Constants.User.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task GetSubscription_WhenGettingForSelfButWithRequiredPermissions_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            permissions: [Permission.Subscription.Get],
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionQueryFactory.CreateGetSubscriptionQuery(
            userId: Constants.User.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorOr.ErrorType.Unauthorized);
    }
}