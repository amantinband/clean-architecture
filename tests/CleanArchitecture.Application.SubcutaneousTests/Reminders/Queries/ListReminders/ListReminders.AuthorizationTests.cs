using CleanArchitecture.Application.Common.Security.Permissions;
using CleanArchitecture.Application.Common.Security.Roles;

namespace CleanArchitecture.Application.SubcutaneousTests.Reminders.Queries.ListReminders;

public class ListRemindersAuthorizationTests
{
    private readonly IMediator _mediator;
    private readonly TestCurrentUserProvider _currentUserProvider;

    public ListRemindersAuthorizationTests()
    {
        var webAppFactory = new WebAppFactory();
        _mediator = webAppFactory.CreateMediator();
        _currentUserProvider = webAppFactory.TestCurrentUserProvider;
    }

    [Fact]
    public async Task ListRemindersForDifferentUser_WhenIsAdmin_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: [Role.Admin]);

        _currentUserProvider.Returns(currentUser);

        var query = ReminderQueryFactory.CreateListRemindersQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task ListRemindersForDifferentUser_WhenIsNotAdmin_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            id: Guid.NewGuid(),
            roles: []);

        _currentUserProvider.Returns(currentUser);

        var query = ReminderQueryFactory.CreateListRemindersQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task ListRemindersForSelf_WhenHasRequiredPermission_ShouldAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            roles: [],
            permissions: [Permission.Reminder.Get]);

        _currentUserProvider.Returns(currentUser);

        var query = ReminderQueryFactory.CreateListRemindersQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.FirstError.Type.Should().NotBe(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task ListRemindersForSelf_WhenDoesNotHaveRequiredPermission_ShouldNotAuthorize()
    {
        // Arrange
        var currentUser = CurrentUserFactory.CreateCurrentUser(
            roles: [],
            permissions: []);

        _currentUserProvider.Returns(currentUser);

        var query = ReminderQueryFactory.CreateListRemindersQuery();

        // Act
        var result = await _mediator.Send(query);

        // Assert
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
    }
}