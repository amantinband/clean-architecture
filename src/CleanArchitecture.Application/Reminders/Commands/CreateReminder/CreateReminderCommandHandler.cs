using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.CreateReminder;

public class CreateReminderCommandHandler(IUsersRepository _usersRepository, ICurrentUserProvider _currentUserProvider)
    : IRequestHandler<CreateReminderCommand, ErrorOr<Reminder>>
{
    public async Task<ErrorOr<Reminder>> Handle(CreateReminderCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();
        var user = await _usersRepository.GetByIdAsync(currentUser.Id);

        if (user is null)
        {
            return Error.Unexpected(description: "User corresponding to current user not found");
        }

        var reminder = new Reminder(user.Id, command.Text, command.DateTime);
        var setReminderResult = user.SetReminder(reminder);

        if (setReminderResult.IsError)
        {
            return setReminderResult.Errors;
        }

        return reminder;
    }
}