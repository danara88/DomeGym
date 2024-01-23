using FluentValidation;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

/// <summary>
/// Delete subscription command validator (model validation)
/// </summary>
public class DeleteSubscriptionCommandValidator : AbstractValidator<DeleteSubscriptionCommand>
{
    public DeleteSubscriptionCommandValidator()
    {
        RuleFor(x => x.SubscriptionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
