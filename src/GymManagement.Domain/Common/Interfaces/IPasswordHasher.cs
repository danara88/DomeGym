using ErrorOr;

namespace GymManagement.Domain.Common.Interfaces;

/// <summary>
/// Password hasher interface
/// </summary>
public interface IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password);

    public bool IsCorrectPassword(string password, string hash);
}
