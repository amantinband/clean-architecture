using System.Data;

using CleanArchitecture.Domain.Reminders;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CleanArchitecture.Infrastructure.Common.Persistence;

public class ListOfReminderIdsConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<List<ReminderId>, string>(
        v => string.Join(',', v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => ReminderId.TryCreate(x).Value).ToList(),
        mappingHints)
{
}

public class ListOfReminderIdsComparer : ValueComparer<List<ReminderId>>
{
    public ListOfReminderIdsComparer()
        : base(
            (t1, t2) => t1!.SequenceEqual(t2!),
            t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
            t => t)
    {
    }
}