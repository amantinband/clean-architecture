using Ardalis.SmartEnum;

namespace CleanArchitecture.Domain.Users;

public class PlanType(string name, int value)
    : SmartEnum<PlanType>(name, value)
{
    public static PlanType Basic = new(nameof(Basic), 0);
    public static PlanType Pro = new(nameof(Pro), 1);

    public int GetMaxDailyReminders() => Name switch
    {
        nameof(Basic) => 3,
        nameof(Pro) => int.MaxValue,
        _ => throw new InvalidOperationException(),
    };
}