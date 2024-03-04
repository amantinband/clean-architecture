using MediatR;

namespace CleanArchitecture.Application.Common.Security.Request;

public interface IAuthorizeableRequest<T> : IRequest<T>
{
}