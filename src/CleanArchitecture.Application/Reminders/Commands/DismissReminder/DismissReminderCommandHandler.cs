using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Reminders.Commands.DismissReminder;

public class DismissReminderCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<DismissReminderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DismissReminderCommand request, CancellationToken cancellationToken) =>
        await (await _usersRepository.GetByIdAsync(request.UserId, cancellationToken)).ToErrorOr()
            .When(user => user is null, Error.NotFound("User not found"))
            .Map(user => user!.DismissReminder(request.ReminderId).Map(success => user!))
            .TapAsync(user => _usersRepository.UpdateAsync(user, cancellationToken))
            .Map(_ => Result.Success);
}