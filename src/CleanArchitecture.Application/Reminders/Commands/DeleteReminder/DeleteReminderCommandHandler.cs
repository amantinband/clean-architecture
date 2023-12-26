using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IRemindersRepository _remindersRepository,
    IUsersRepository _usersRepository) : IRequestHandler<DeleteReminderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _remindersRepository.GetByIdAsync(request.ReminderId, cancellationToken);

        var user = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (reminder is null || user is null)
        {
            return Error.NotFound(description: "Reminder not found");
        }

        var deleteReminderResult = user.DeleteReminder(reminder);

        if (deleteReminderResult.IsError)
        {
            return deleteReminderResult.Errors;
        }

        await _usersRepository.UpdateAsync(user, cancellationToken);

        return Result.Success;
    }
}