using System.Security.Claims;
using GymManagement.Application.Common.Interfaces;
using Throw;

namespace GymManagement.Api.Services;

/// <summary>
/// Current User Provdier implementation
/// </summary>
public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccesor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccesor)
    {
        _httpContextAccesor = httpContextAccesor;
    }

    /// <summary>
    /// This method gets the current user id
    /// </summary>
    /// <returns></returns>
    public CurrentUser GetCurrentUser()
    {
        _httpContextAccesor.HttpContext.ThrowIfNull();

        var id = GetClaimValues("id")
            .Select(value => Guid.Parse(value))
            .First();

        var permissions = GetClaimValues("permissions");

        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(
            Id: id,
            Permissions: permissions,
            Roles: roles
        );
    }

    /// <summary>
    /// Gets the the claims values from the token
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return _httpContextAccesor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
