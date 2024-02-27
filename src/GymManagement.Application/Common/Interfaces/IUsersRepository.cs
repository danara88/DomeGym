using GymManagement.Domain.Users;

namespace GymManagement.Application.Common.Interfaces;

/// <summary>
/// Users repository interface
/// </summary>
public interface IUsersRepository
{
    Task AddUserAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid userId);
    Task UpdateAsync(User user);
}
