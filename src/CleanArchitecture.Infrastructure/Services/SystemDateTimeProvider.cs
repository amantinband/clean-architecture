using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure.Services;
<!--fetch date time-->

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
