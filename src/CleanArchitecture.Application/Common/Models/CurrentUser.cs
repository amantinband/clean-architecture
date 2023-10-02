namespace CleanArchitecture.Application.Common.Models;

public class CurrentUser(Guid id)
{
    public Guid Id { get; } = id;
}