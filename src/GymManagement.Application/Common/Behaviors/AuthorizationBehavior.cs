using System.Reflection;
using ErrorOr;
using GymManagement.Application.Common.Authorization;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

/// <summary>
/// Authorization behavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class AuthorizationBehavior<TRequest, TResponse>
  : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
      where TResponse : IErrorOr
{
    private readonly ICurrentUserProvider _currentUserProvider;

    public AuthorizationBehavior(ICurrentUserProvider currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        // Example:
        // atrribute1 { permissions: "[permission1, permission2]" }
        // atrribute2 { permissions: "[permission3, permission4]" }
        // atrribute3 { permissions: null }
        // requiredPermissions = ["permission1", "permission2", "permission3", "permisssion4"]
        List<string> requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? Enumerable.Empty<string>())
            .ToList();

        var currentUser = _currentUserProvider.GetCurrentUser();

        // If the current user permissions has differences with the required permissions
        // then return unauthorize.
        // Example:
        // requiredPermissions = ["create:gym""]
        // currentUserPermissions = ["create:gym", "update:gym"]
        // The differences between currentUserPermissions and requiredPermissions are 0 because both has create:gym permission
        if (requiredPermissions.Except(currentUser.Permissions).Count() > 0)
        {
            // return Error.Unauthorized(description: "User is forbidden from taking this action");
            return (dynamic)Error.Failure(description: "User is forbidden from taking this action");
        }

         List<string> requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? Enumerable.Empty<string>())
            .ToList();

         if (requiredRoles.Except(currentUser.Roles).Count() > 0)
        {
            // return Error.Unauthorized(description: "User is forbidden from taking this action");
            return (dynamic)Error.Failure(description: "User is forbidden from taking this action");
        }

        return await next();
    }
}
