using CleanArchitecture.Application.Common.Interfaces;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IRemindersRepository _remindersRepository,
    IUsersRepository _usersRepository) : IRequestHandler<DeleteReminderCommand, Result<FunctionalDdd.Unit>>
{
    public async Task<Result<FunctionalDdd.Unit>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken) =>
        await _remindersRepository.GetByIdAsync(request.ReminderId, cancellationToken).ToResultAsync(Error.NotFound("Reminder not found"))
            .ParallelAsync(_usersRepository.GetByIdAsync(request.UserId, cancellationToken).ToResultAsync(Error.NotFound("User not found")))
            .BindAsync((reminder, user) => user.DeleteReminder(reminder).Map(_ => user))
            .BindAsync(user =>
            {
                _usersRepository.UpdateAsync(user, cancellationToken);
                return Result.Success();
            });
}