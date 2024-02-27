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

        var idClaim = _httpContextAccesor
          .HttpContext.User.Claims
          .First(claim => claim.Type == "id");

         var permissions = _httpContextAccesor
          .HttpContext.User.Claims
          .Where(claim => claim.Type == "permissions")
          .Select(claim => claim.Value)
          .ToList();

        return new CurrentUser(
            Id: Guid.Parse(idClaim.Value),
            Permissions: permissions
        );
    }
}
