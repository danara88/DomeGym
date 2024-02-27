namespace GymManagement.Application.Common.Interfaces;

/// <summary>
/// Current user provider interface
/// </summary>
public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}
