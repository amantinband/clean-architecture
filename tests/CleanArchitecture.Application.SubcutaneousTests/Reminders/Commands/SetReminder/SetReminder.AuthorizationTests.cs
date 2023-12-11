using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;

namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Commands.SetReminder;

public class SetReminderAuthorizationTests
{
    private readonly IMediator _mediator;
    private readonly TestCurrentUserProvider _currentUserProvider;

    public SetReminderAuthorizationTests()
    {
        var webAppFactory = new WebAppFactory();
        _mediator = webAppFactory.CreateMediator();
        _currentUserProvider = webAppFactory.TestCurrentUserProvider;
    }

    [Fact]
    public async Task SetReminder_WhenDifferentUserButWithAdminRole_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var command = ReminderCommandFactory.CreateSetReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task SetReminder_WhenDifferentUserWithoutAdminRole_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = ReminderCommandFactory.CreateSetReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task SetReminder_WhenSettingForSelfWithRequiredPermissions_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            permissions: [Permission.Reminder.Set],
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = ReminderCommandFactory.CreateSetReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task SetReminder_WhenSettingForSelfButWithoutRequiredPermissions_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            permissions: [],
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var command = ReminderCommandFactory.CreateSetReminderCommand();

        // Act
        var result = await _mediator.Send(command);

        // Assert
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }
}