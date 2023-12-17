using FluentValidation;

namespace CleanArchitecture.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MinimumLength(2)
            .MaximumLength(10000);

        RuleFor(x => x.LastName)
            .MinimumLength(2)
            .MaximumLength(10000);

        RuleFor(x => x.Email).EmailAddress();
    }
}