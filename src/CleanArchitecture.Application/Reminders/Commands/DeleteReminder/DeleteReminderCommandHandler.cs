using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IRemindersRepository _remindersRepository,
    IUsersRepository _usersRepository) : IRequestHandler<DeleteReminderCommand, Result<FunctionalDdd.Unit>>
{
    public async Task<Result<FunctionalDdd.Unit>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        var hReminderId = ReminderId.TryCreate(request.ReminderId);
        if (hReminderId.IsFailure)
        {
            return hReminderId.Error;
        }

        return await UserId.TryCreate(request.UserId)
                .BindAsync(userId => _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("User not found")))
                .ParallelAsync(_remindersRepository.GetByIdAsync(hReminderId.Value, cancellationToken).ToResultAsync(Error.NotFound("Reminder not found")))
                .BindAsync((user, reminder) => user.DeleteReminder(reminder).Map(_ => user))
                .BindAsync(user =>
                {
                    _usersRepository.UpdateAsync(user, cancellationToken);
                    return Result.Success();
                });
    }
}