using GymManagement.Api.Services;
using GymManagement.Application.Common.Interfaces;

namespace GymManagement.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Configure ProblemDetails middleware. Establish standard format for
        // returning deatails about HTTP errors.
        services.AddProblemDetails();

        // HttpContextAccessor is a singleton
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}
