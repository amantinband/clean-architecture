using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Users;

using MediatR;
using Unit = FunctionalDdd.Unit;

namespace CleanArchitecture.Application.Reminders.Commands.DismissReminder;

public class DismissReminderCommandHandler(
    IUsersRepository _usersRepository)
        : IRequestHandler<DismissReminderCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DismissReminderCommand request, CancellationToken cancellationToken) =>
        await ReminderId.TryCreate(request.ReminderId)
             .Combine(UserId.TryCreate(request.UserId))
             .BindAsync((reminderId, userId) =>
                    _usersRepository.GetByIdAsync(userId, cancellationToken).ToResultAsync(Error.NotFound("Reminder not found."))
                    .BindAsync(user => user.DismissReminder(reminderId)
                        .TapAsync(_ => _usersRepository.UpdateAsync(user, cancellationToken))));
}
