using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using MediatR;

using Unit = FunctionalDdd.Unit;

namespace CleanArchitecture.Application.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IRemindersRepository _remindersRepository,
    IUsersRepository _usersRepository) : IRequestHandler<DeleteReminderCommand, Result<FunctionalDdd.Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken) =>
        await UserId.TryCreate(request.UserId)
            .Combine(ReminderId.TryCreate(request.ReminderId))
            .BindAsync((userId, reminderId) =>
                    _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("User not found"))
                    .ParallelAsync(_remindersRepository.GetByIdAsync(reminderId, cancellationToken).ToResultAsync(Error.NotFound("Reminder not found")))
                    .BindAsync((user, reminder) =>
                        user.DeleteReminder(reminder)
                        .TapAsync(_ => _usersRepository.UpdateAsync(user, cancellationToken))));
}