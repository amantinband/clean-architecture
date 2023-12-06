using System.Text.Json.Serialization;

namespace CleanArchitecture.Contracts.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Basic,
    Pro,
}