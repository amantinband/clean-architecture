using CleanArchitecture.Application.Reminders.Commands.CreateReminder;
using CleanArchitecture.Application.Reminders.Queries.GetReminder;
using CleanArchitecture.Contracts.Reminders;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("reminders")]
public class RemindersController(ISender _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateReminder(CreateReminderRequest request)
    {
        var createReminderCommand = new CreateReminderCommand(request.Text, request.DateTime);

        var createReminderResult = await _mediator.Send(createReminderCommand);

        return createReminderResult.Match(
            reminder => CreatedAtAction(
                actionName: nameof(GetReminder),
                routeValues: new { ReminderId = reminder.Id },
                value: reminder),
            Problem);
    }

    [HttpGet("{reminderId:guid}")]
    public async Task<IActionResult> GetReminder(Guid reminderId)
    {
        var getReminderQuery = new GetReminderQuery(reminderId);

        var getReminderResult = await _mediator.Send(getReminderQuery);

        return getReminderResult.Match(
            reminder => Ok(new ReminderResponse(reminder.Text, reminder.DateTime)),
            Problem);
    }
}