using FluentValidation;
namespace GymManagement.Application.Gyms.Commands.CreateGym;

/// <summary>
/// Create gym command validator (model validation)
/// </summary>
public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
  public CreateGymCommandValidator()
  {
    RuleFor(x => x.Name)
      .MinimumLength(3)
      .MaximumLength(100);
  }
}
