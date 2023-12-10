using CleanArchitecture.Application.Common.Security.Roles;
using CleanArchitecture.Application.SubcutaneousTests.Common;

using FluentAssertions;

using MediatR;

using TestCommon.Security;
using TestCommon.Subscriptions;
using TestCommon.TestConstants;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

public class CancelSubscriptionAuthorizationTests(MediatorFactory mediatorFactory)
    : IClassFixture<MediatorFactory>
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    private readonly TestCurrentUserProvider _currentUserProvider = mediatorFactory.TestCurrentUserProvider;

    public static TheoryData<Guid> ListGuids() => [];

    [Fact]
    public async Task CancelSubscription_WhenCancelingForSelfWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(
            userId: Constants.User.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CancelSubscription_WhenCancelingForSelfWithoutAdminRole_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Constants.User.Id,
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(
            userId: Constants.User.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CancelSubscription_WhenDifferentUserWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(
            userId: Constants.User.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorOr.ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CancelSubscription_WhenDifferentUserWithoutAdminRole_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand(
            userId: Constants.User.Id);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Unauthorized);
    }
}