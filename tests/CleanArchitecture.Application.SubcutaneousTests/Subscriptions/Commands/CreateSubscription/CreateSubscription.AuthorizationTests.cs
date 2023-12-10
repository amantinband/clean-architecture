using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Application.SubcutaneousTests.Common;

using FluentAssertions;

using MediatR;

using TestCommon.Security;
using TestCommon.Subscriptions;
using TestCommon.TestConstants;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionAuthorizationTests(MediatorFactory mediatorFactory)
    : IClassFixture<MediatorFactory>
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    private readonly TestCurrentUserProvider _currentUserProvider = mediatorFactory.TestCurrentUserProvider;

    [Fact]
    public async Task CreateSubscription_WhenDifferentUserButWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(
            userId: Guid.NewGuid(),
            subscriptionType: Constants.Subscription.Type);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CreateSubscription_WhenCreatingForSelfWithRequiredPermission_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            roles: [],
            permissions: [Permission.Subscription.Create]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand(
            userId: Constants.User.Id,
            subscriptionType: Constants.Subscription.Type);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CreateSubscription_WhenUserHasNoRolesOrPermissions_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(roles: [], permissions: []);
        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Unauthorized);
    }
}