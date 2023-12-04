using CleanArchitecture.Application.Users.Commands.CreateUser;
using CleanArchitecture.Application.Users.Queries.GetUser;
using CleanArchitecture.Contracts.Users;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using DomainPlanType = CleanArchitecture.Domain.Users.PlanType;

namespace CleanArchitecture.Api.Controllers;

[Route("users")]
public class UsersController(IMediator _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        if (!DomainPlanType.TryFromName(request.PlanType.ToString(), out var planType))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Invalid plan type");
        }

        var createUserCommand = new CreateUserCommand(planType, request.FullName);

        var createUserResult = await _mediator.Send(createUserCommand);

        return createUserResult.Match(
            user => CreatedAtAction(
                actionName: nameof(GetUser),
                routeValues: new { UserId = user.Id },
                value: new UserResponse(user.Id, user.FullName, ToDto(user.Plan))),
            Problem);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var getUserQuery = new GetUserQuery(userId);

        var getUserResult = await _mediator.Send(getUserQuery);

        return getUserResult.Match(
            user => Ok(new UserResponse(user.Id, user.FullName, ToDto(user.Plan))),
            Problem);
    }

    private static PlanType ToDto(DomainPlanType subscriptionType)
    {
        return subscriptionType.Name switch
        {
            nameof(DomainPlanType.Basic) => PlanType.Basic,
            nameof(DomainPlanType.Pro) => PlanType.Pro,
            _ => throw new InvalidOperationException(),
        };
    }
}