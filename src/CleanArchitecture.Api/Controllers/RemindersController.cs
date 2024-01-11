using CleanArchitecture.Application.Reminders.Commands.DeleteReminder;
using CleanArchitecture.Application.Reminders.Commands.DismissReminder;
using CleanArchitecture.Application.Reminders.Commands.SetReminder;
using CleanArchitecture.Application.Reminders.Queries.GetReminder;
using CleanArchitecture.Application.Reminders.Queries.ListReminders;
using CleanArchitecture.Contracts.Reminders;
using CleanArchitecture.Domain.Reminders;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("users/{userId:guid}/subscriptions/{subscriptionId:guid}/reminders")]
public class RemindersController(ISender _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateReminder(Guid userId, Guid subscriptionId, CreateReminderRequest request) =>
        await new SetReminderCommand(userId, subscriptionId, request.Text, request.DateTime.UtcDateTime).ToErrorOr()
            .MapAsync(command => _mediator.Send(command))
            .Map(ToDto)
            .Match(
                reminderResponse => CreatedAtAction(
                    actionName: nameof(GetReminder),
                    routeValues: new { UserId = userId, SubscriptionId = subscriptionId, ReminderId = reminderResponse.Id },
                    value: reminderResponse),
                Problem);

    [HttpPost("{reminderId:guid}/dismiss")]
    public async Task<IActionResult> DismissReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new DismissReminderCommand(userId, subscriptionId, reminderId).ToErrorOr()
            .MapAsync(command => _mediator.Send(command))
            .Match(_ => NoContent(), Problem);

    [HttpDelete("{reminderId:guid}")]
    public async Task<IActionResult> DeleteReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new DeleteReminderCommand(userId, subscriptionId, reminderId).ToErrorOr()
            .MapAsync(command => _mediator.Send(command))
            .Match(_ => NoContent(), Problem);

    [HttpGet("{reminderId:guid}")]
    public async Task<IActionResult> GetReminder(Guid userId, Guid subscriptionId, Guid reminderId) =>
        await new GetReminderQuery(userId, subscriptionId, reminderId).ToErrorOr()
            .MapAsync(query => _mediator.Send(query))
            .Match(Ok, Problem);

    [HttpGet]
    public async Task<IActionResult> ListReminders(Guid userId, Guid subscriptionId) =>
        await new ListRemindersQuery(userId, subscriptionId).ToErrorOr()
            .MapAsync(query => _mediator.Send(query))
            .Map(reminders => reminders.ConvertAll(ToDto))
            .Match(Ok, Problem);

    private ReminderResponse ToDto(Reminder reminder) =>
        new(reminder.Id, reminder.Text, reminder.DateTime, reminder.IsDismissed);
}