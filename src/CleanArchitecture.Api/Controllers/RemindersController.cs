using CleanArchitecture.Application.Common.Interfaces;
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

using Unit = FunctionalDdd.Unit;

namespace CleanArchitecture.Api.Controllers;
[ApiController]
[Authorize]
[Route("users/{userId:guid}/subscriptions/{subscriptionId:guid}/reminders")]
public class RemindersController(ISender _mediator, IDateTimeProvider _dateTimeProvider) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Unit>> CreateReminder(Guid userId, Guid subscriptionId, CreateReminderRequest request) =>
        await SetReminderCommand.TryCreate(userId, subscriptionId, request.Text, request.DateTime.UtcDateTime, _dateTimeProvider)
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
             reminder => CreatedAtAction(
                actionName: nameof(GetReminder),
                routeValues: new { UserId = userId, SubscriptionId = subscriptionId, ReminderId = reminder.Id },
                value: ToDto(reminder)),
             err => err.ToErrorActionResult<Unit>(this));

    [HttpPost("{reminderId:guid}/dismiss")]
    public async Task<ActionResult<Unit>> DismissReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new DismissReminderCommand(userId, subscriptionId, reminderId).ToResult()
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
                _ => NoContent(),
                err => err.ToErrorActionResult<Unit>(this));

    [HttpDelete("{reminderId:guid}")]
    public async Task<ActionResult<Unit>> DeleteReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new DeleteReminderCommand(userId, subscriptionId, reminderId).ToResult()
            .BindAsync(command => _mediator.Send(command))
            .FinallyAsync(
                _ => NoContent(),
                err => err.ToErrorActionResult<Unit>(this));

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