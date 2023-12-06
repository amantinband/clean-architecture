using CleanArchitecture.Contracts.Common;

namespace CleanArchitecture.Contracts.Subscriptions;

public record SubscriptionResponse(
    Guid Id,
    Guid UserId,
    string UserFirstName,
    string UserLastName,
    SubscriptionType SubscriptionType);