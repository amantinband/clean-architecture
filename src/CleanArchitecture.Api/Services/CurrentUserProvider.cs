using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Api.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        return new CurrentUser(Guid.Parse("0FEC059B-9A78-4BDD-A01D-6A25D30DEE08"));
    }
}