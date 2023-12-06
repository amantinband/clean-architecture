using Ardalis.SmartEnum;

namespace CleanArchitecture.Domain.Users;

public class SubscriptionType(string name, int value)
    : SmartEnum<SubscriptionType>(name, value)
{
    public static readonly SubscriptionType Basic = new(nameof(Basic), 0);
    public static readonly SubscriptionType Pro = new(nameof(Pro), 1);

    public int GetMaxDailyReminders() => Name switch
    {
        nameof(Basic) => 3,
        nameof(Pro) => int.MaxValue,
        _ => throw new InvalidOperationException(),
    };
}