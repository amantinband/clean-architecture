using System.Text.Json.Serialization;

namespace CleanArchitecture.Contracts.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Basic,
    Pro,
}