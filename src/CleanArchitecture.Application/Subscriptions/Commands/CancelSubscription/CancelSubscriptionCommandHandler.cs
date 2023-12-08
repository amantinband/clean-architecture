using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(IUsersRepository _usersRepository)
    : IRequestHandler<CancelSubscriptionCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        var deleteSubscriptionResult = user.CancelSubscription(request.SubscriptionId);

        if (deleteSubscriptionResult.IsError)
        {
            return deleteSubscriptionResult.Errors;
        }

        await _usersRepository.UpdateAsync(user, cancellationToken);

        return Result.Success;
    }
}
