using CleanArchitecture.Domain.Users;

using GymManagement.Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Users.Persistence;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property("_maxDailyReminders")
            .HasColumnName("MaxDailyReminders");

        builder.OwnsOne<Calendar>("_calendar", cb =>
        {
            cb.WithOwner().HasForeignKey("UserId");

            cb.Property<Dictionary<DateOnly, int>>("_calendar")
                .HasColumnName("CalendarDictionary")
                .HasValueJsonConverter();
        });

        builder.Property<List<Guid>>("_reminderIds")
            .HasColumnName("ReminderIds")
            .HasListOfIdsConverter();

        builder.Property(u => u.FullName);

        builder.Property(u => u.Plan)
            .HasConversion(
                plan => plan.Name,
                name => PlanType.FromName(name, false));
    }
}