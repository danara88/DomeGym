using FluentValidation;

namespace GymManagement.Application.Gyms.Commands.DeleteGym;

/// <summary>
/// Delete gym command validator (model validation)
/// </summary>
public class DeleteGymCommandValidator : AbstractValidator<DeleteGymCommand>
{
    public DeleteGymCommandValidator()
    {
        RuleFor(x => x.SubscriptionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.SubscriptionId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
