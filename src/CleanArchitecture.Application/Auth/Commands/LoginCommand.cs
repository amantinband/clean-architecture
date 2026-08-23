using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitecture.Application.Auth.Dto;
using CleanArchitecture.Application.Common.Interfaces;

using ErrorOr;

using MediatR;

using OneOf.Types;

namespace CleanArchitecture.Application.Auth.Commands;

public class LoginCommand : LoginRequest, IRequest<ErrorOr<Success<LoginResponse>>>
{
}

public class LoginCommandHandler(
    IUsersRepository usersRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, ErrorOr<Success<LoginResponse>>>
{
    public async Task<ErrorOr<Success<LoginResponse>>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var users = await usersRepository.GetByEmailsAsync(new List<string> { request.Email });
        var user = users.FirstOrDefault();
        if (user == null)
        {
            return ErrorOr.Error.NotFound();
        }

        var accessToken = await jwtTokenGenerator.GenerateTokenAsync(user.Id, new[] { "get:reminder", "get:subscription" }, new[] { "admin" }, cancellationToken);
        var refreshToken = Guid.NewGuid().ToString();
        return new Success<LoginResponse>(new LoginResponse(accessToken, refreshToken));
    }
}
