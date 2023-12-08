using CleanArchitecture.Domain.Reminders;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Reminders.Persistence;

public class ReminderConfigurations : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.SubscriptionId);

        builder.Property(r => r.DateTime);

        builder.Property(r => r.Text);
    }
}
