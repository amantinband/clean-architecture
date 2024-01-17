namespace CleanArchitecture.Domain.Common;

public abstract class CreationEntity : Entity
{
    public Guid UserId { get; private init; }

    public DateTime DateTime { get; }

    public DateOnly Date => DateOnly.FromDateTime(DateTime.Date);

    protected CreationEntity(Guid id, Guid userId, DateTime dateTime)
        : base(id)
    {
        UserId = userId;
        DateTime = dateTime;
    }

    protected CreationEntity() { }
}