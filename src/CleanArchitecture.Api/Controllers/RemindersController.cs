using CleanArchitecture.Application.Reminders.Commands.DeleteReminder;
using CleanArchitecture.Application.Reminders.Commands.DismissReminder;
using CleanArchitecture.Application.Reminders.Commands.SetReminder;
using CleanArchitecture.Application.Reminders.Queries.GetReminder;
using CleanArchitecture.Application.Reminders.Queries.ListReminders;
using CleanArchitecture.Contracts.Reminders;
using CleanArchitecture.Domain.Reminders;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;
[ApiController]
[Authorize]
[Route("users/{userId:guid}/subscriptions/{subscriptionId:guid}/reminders")]
public class RemindersController(ISender _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<FunctionalDdd.Unit>> CreateReminder(Guid userId, Guid subscriptionId, CreateReminderRequest request) =>
        await new SetReminderCommand(userId, subscriptionId, request.Text, request.DateTime.UtcDateTime).ToResult()
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
             reminder => CreatedAtAction(
                actionName: nameof(GetReminder),
                routeValues: new { UserId = userId, SubscriptionId = subscriptionId, ReminderId = reminder.Id },
                value: ToDto(reminder)),
             err => err.ToErrorActionResult<FunctionalDdd.Unit>(this));

    [HttpPost("{reminderId:guid}/dismiss")]
    public async Task<ActionResult<FunctionalDdd.Unit>> DismissReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new DismissReminderCommand(userId, subscriptionId, reminderId).ToResult()
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
                _ => NoContent(),
                err => err.ToErrorActionResult<FunctionalDdd.Unit>(this));

    [HttpDelete("{reminderId:guid}")]
    public async Task<ActionResult<FunctionalDdd.Unit>> DeleteReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new DeleteReminderCommand(userId, subscriptionId, reminderId).ToResult()
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
                _ => NoContent(),
                err => err.ToErrorActionResult<FunctionalDdd.Unit>(this));

    [HttpGet("{reminderId:guid}")]
    public async Task<ActionResult<ReminderResponse>> GetReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new GetReminderQuery(userId, subscriptionId, reminderId).ToResult()
            .BindAsync(query => _mediator.Send(query))
            .MapAsync(ToDto)
            .ToOkActionResultAsync(this);

    [HttpGet]
    public async Task<ActionResult<List<ReminderResponse>>> ListReminders(Guid userId, Guid subscriptionId) =>
        await new ListRemindersQuery(userId, subscriptionId).ToResult()
            .BindAsync(query => _mediator.Send(query))
            .MapAsync(reminders => reminders.ConvertAll(ToDto))
            .ToOkActionResultAsync(this);

    private ReminderResponse ToDto(Reminder reminder) =>
        new(reminder.Id, reminder.Text, reminder.DateTime, reminder.IsDismissed);
}