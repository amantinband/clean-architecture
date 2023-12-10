using CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;
using CleanArchitecture.Application.Subscriptions.Common;
using CleanArchitecture.Application.Subscriptions.Queries.GetSubscription;

using ErrorOr;

using FluentAssertions;

using MediatR;

namespace TestCommon.Subscriptions;

public static class MediatorExtensions
{
    public static async Task<SubscriptionResult> CreateSubscription(
        this IMediator mediator,
        CreateSubscriptionCommand? command = null)
    {
        var result = await mediator.Send(command ?? SubscriptionCommandFactory.CreateCreateSubscriptionCommand());

        result.IsError.Should().BeFalse();

        return result.Value;
    }

    public static async Task<ErrorOr<SubscriptionResult>> GetSubscription(
        this IMediator mediator,
        GetSubscriptionQuery? query = null)
    {
        return await mediator.Send(query ?? SubscriptionQueryFactory.CreateGetSubscriptionQuery());
    }
}