using FluentValidation;

namespace GymManagement.Application.Gyms.Commands.AddTrainer;

/// <summary>
/// Add trainer command validator (model validation)
/// </summary>
public class AddTrainerCommandValidator : AbstractValidator<AddTrainerCommand>
{
    public AddTrainerCommandValidator()
    {
       RuleFor(x => x.SubscriptionId)
          .NotEmpty()
          .NotEqual(Guid.Empty);

      RuleFor(x => x.GymId)
          .NotEmpty()
          .NotEqual(Guid.Empty);

      RuleFor(x => x.TrainerId)
          .NotEmpty()
          .NotEqual(Guid.Empty);
    }
}
