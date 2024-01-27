using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;

namespace CleanArchitecture.Application.SubcutaneousTests.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionAuthorizationTests
{
    private readonly IMediator _mediator;
    private readonly TestCurrentUserProvider _currentUserProvider;

    public CreateSubscriptionAuthorizationTests()
    {
        var webAppFactory = new WebAppFactory();
        _mediator = webAppFactory.CreateMediator();
        _currentUserProvider = webAppFactory.TestCurrentUserProvider;
    }

    [Fact]
    public async Task CreateSubscriptionForDifferentUser_WhenIsAdmin_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateSubscriptionForDifferentUser_WhenIsNotAdmin_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.Error.Should().BeOfType<ForbiddenError>();
    }

    [Fact]
    public async Task CreateSubscriptionForSelf_WhenHasRequiredPermission_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            roles: [],
            permissions: [Permission.Subscription.Create]);

        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateSubscriptionForSelf_WhenDoesNotHaveRequiredPermission_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            roles: [],
            permissions: []);
        _currentUserProvider.Returns(currentUser);

        var command = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.Error.Should().BeOfType<ForbiddenError>();
    }
}