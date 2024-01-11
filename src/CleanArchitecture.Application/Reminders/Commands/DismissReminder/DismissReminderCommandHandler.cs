using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DismissReminder;

public class DismissReminderCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<DismissReminderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DismissReminderCommand request, CancellationToken cancellationToken) =>
        await (await _usersRepository.GetByIdAsync(request.UserId, cancellationToken)).ToErrorOr()
            .FailIf(user => user is null, Error.NotFound("User not found"))
            .Then(user => user!.DismissReminder(request.ReminderId).Then(success => user!))
            .ThenDoAsync(user => _usersRepository.UpdateAsync(user, cancellationToken))
            .Then(_ => Result.Success);
}