using FluentValidation;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

/// <summary>
/// Create subscription command validator (model validation)
/// </summary>
public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(x => x.SubscriptionType)
            .NotEmpty();

        RuleFor(x => x.AdminId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
