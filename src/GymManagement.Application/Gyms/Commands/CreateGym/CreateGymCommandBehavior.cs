using ErrorOr;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

/// <summary>
/// Create gym command behavior
/// NOTE: This is an example, it is not used in the system.
/// </summary>
public class CreateGymCommandBehavior : IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(
      CreateGymCommand request,
      RequestHandlerDelegate<ErrorOr<Gym>> next,
      CancellationToken cancellationToken)
    {
        var validator = new CreateGymCommandValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
                .ToList();
        }

        // Executes the handler
        return await next();
    }
}
