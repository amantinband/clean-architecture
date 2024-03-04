namespace CleanArchitecture.Application.Common.Security.Request;

public interface IUserAuthorizeableRequest<T> : IAuthorizeableRequest<T>
{
    Guid UserId { get; }
}