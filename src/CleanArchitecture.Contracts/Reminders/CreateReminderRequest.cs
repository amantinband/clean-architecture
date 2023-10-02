namespace CleanArchitecture.Contracts.Reminders;

public record CreateReminderRequest(string Text, DateTimeOffset DateTime);