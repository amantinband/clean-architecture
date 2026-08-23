using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitecture.Application.Auth.Dto;
using CleanArchitecture.Application.Common.Interfaces;

using MediatR;

namespace CleanArchitecture.Application.Auth.Commands;

public class RefreshTokenCommand : IRequest<LoginResponse>
{
    public required string RefreshToken { get; init; }
    public required Guid UserId { get; init; }
}

internal class RefreshTokenCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // RefreshTokenAsync will validate the refresh token and return new tokens
        var (accessToken, refreshToken) = await jwtTokenGenerator.RefreshTokenAsync(request.RefreshToken, request.UserId, cancellationToken: cancellationToken);

        return new LoginResponse(accessToken, refreshToken);
    }
}
