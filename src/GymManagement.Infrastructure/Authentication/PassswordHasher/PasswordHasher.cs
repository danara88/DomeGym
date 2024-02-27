using System.Text.RegularExpressions;
using ErrorOr;
using GymManagement.Domain.Common.Interfaces;

namespace GymManagement.Infrastructure.Authentication.PasswordHasher;

/// <summary>
/// Password Hasher
/// </summary>
public partial class PasswordHasher : IPasswordHasher
{
    private static readonly Regex PasswordRegex = StrongPasswordRegex();

    /// <summary>
    /// Method to hash a password
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public ErrorOr<string> HashPassword(string password)
    {
        return !PasswordRegex.IsMatch(password)
            ? Error.Validation(description: "Password to weak")
            : BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    /// <summary>
    /// Method to verify if the password is correct or not
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }

    /// <summary>
    /// Gets a strong password regex
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();

}
