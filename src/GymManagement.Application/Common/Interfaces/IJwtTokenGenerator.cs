using GymManagement.Domain.Users;

namespace GymManagement.Application.Common.Interfaces;

/// <summary>
/// JWT token generator interface
/// </summary>
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
