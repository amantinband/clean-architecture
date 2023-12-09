using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Application.SubcutaneousTests.Common;
using CleanArchitecture.Infrastructure.Common.Security.CurrentUserProvider;

using FluentAssertions;

using MediatR;

using TestCommon.Security;
using TestCommon.Subscriptions;
using TestCommon.TestConstants;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateSubscriptionTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    private readonly TestCurrentUserProvider _currentUserProvider = mediatorFactory.TestCurrentUserProvider;

    [Fact]
    public async Task CreateSubscription_WhenDifferentUserButWithAdminRole_ShouldCreateSubscription()
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
        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(command.UserId);
        result.Value.SubscriptionType.Should().Be(Constants.Subscription.Type);
    }

    [Fact]
    public async Task CreateSubscription_WhenCreatingForSelfWithRequiredPermission_ShouldCreateSubscription()
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
        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(command.UserId);
        result.Value.SubscriptionType.Should().Be(Constants.Subscription.Type);
    }

    [Fact]
    public async Task CreateSubscription_WhenUserHasNoRolesOrPermissions_ShouldReturnUnauthorized()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(roles: [], permissions: []);
        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Unauthorized);
    }
}