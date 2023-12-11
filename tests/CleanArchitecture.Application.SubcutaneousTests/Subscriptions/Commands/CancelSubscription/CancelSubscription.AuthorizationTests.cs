using CleanArchitecture.Application.Common.Security.Roles;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionAuthorizationTests
{
    private readonly IMediator _mediator;
    private readonly TestCurrentUserProvider _currentUserProvider;

    public CancelSubscriptionAuthorizationTests()
    {
        var webAppFactory = new WebAppFactory();
        _mediator = webAppFactory.CreateMediator();
        _currentUserProvider = webAppFactory.TestCurrentUserProvider;
    }

    [Fact]
    public async Task CancelSubscription_WhenCancelingForSelfWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CancelSubscription_WhenCancelingForSelfWithoutAdminRole_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CancelSubscription_WhenDifferentUserWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task CancelSubscription_WhenDifferentUserWithoutAdminRole_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCancelSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }
}