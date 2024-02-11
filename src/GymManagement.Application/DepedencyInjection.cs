using FluentValidation;
using GymManagement.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            // This will scan all the commands/queries that we have in the assembly (Application layer)
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            // Example how we can register a behavior
            // options.AddBehavior<IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>, CreateGymCommandBehavior>();

            // Add generic behavior
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // It goes ahead and scan the assembly (Application layer) and it finds all the validators
        // and register those as IValidator<T> (not as AbstractValidator)
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}
