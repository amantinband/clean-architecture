using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DismissReminder;

public class DismissReminderCommandHandler(
    IUsersRepository _usersRepository)
        : IRequestHandler<DismissReminderCommand, Result<FunctionalDdd.Unit>>
{
    public async Task<Result<FunctionalDdd.Unit>> Handle(DismissReminderCommand request, CancellationToken cancellationToken)
    {
        var hReminderId = ReminderId.TryCreate(request.ReminderId);
        if (hReminderId.IsFailure)
        {
            return hReminderId.Error;
        }

        return await UserId.TryCreate(request.UserId)
                .BindAsync(userId => _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("Reminder not found.")))
                .BindAsync(user => user.DismissReminder(hReminderId.Value).Map(r => user))
                .BindAsync(user =>
                {
                    _usersRepository.UpdateAsync(user, cancellationToken);
                    return Result.Success();
                });
    }
}
