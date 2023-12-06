using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.CreateReminder;

public class CreateReminderCommandHandler(IUsersRepository _usersRepository, ICurrentUserProvider _currentUserProvider)
    : IRequestHandler<CreateReminderCommand, ErrorOr<Reminder>>
{
    public async Task<ErrorOr<Reminder>> Handle(CreateReminderCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        var reminder = new Reminder(currentUser.Id, command.Text, command.DateTime);

        var user = await _usersRepository.GetByIdAsync(currentUser.Id);

        if (user is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        var setReminderResult = user.SetReminder(reminder);

        if (setReminderResult.IsError)
        {
            return setReminderResult.Errors;
        }

        await _usersRepository.UpdateAsync(user);

        return reminder;
    }
}