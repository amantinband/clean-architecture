using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IRemindersRepository _remindersRepository,
    IUsersRepository _usersRepository) : IRequestHandler<DeleteReminderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        Reminder? reminder = await _remindersRepository.GetByIdAsync(request.ReminderId, cancellationToken);
        User? user = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        return await (Reminder: reminder, User: user).ToErrorOr()
                .FailIf(pair => pair.Reminder is null || pair.User is null, Error.NotFound("Reminder not found"))
                .Then(pair => (Reminder: pair.Reminder!, User: pair.User!))
                .Then(pair => pair.User.DeleteReminder(pair.Reminder).Then(success => pair))
                .ThenDoAsync(pair => _usersRepository.UpdateAsync(pair.User, cancellationToken))
                .Then(_ => Result.Success);
    }
}