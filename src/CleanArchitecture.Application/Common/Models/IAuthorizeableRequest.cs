using MediatR;

namespace CleanArchitecture.Application.Common.Models;

public interface IAuthorizeableRequest<T> : IRequest<T>
{
    Guid UserId { get; }
}