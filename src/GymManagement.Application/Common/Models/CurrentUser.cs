/// <summary>
/// Current User Model
/// </summary>
/// <param name="Id"></param>
/// <param name="Permissions"></param>
public record CurrentUser(Guid Id, IReadOnlyList<string> Permissions, IReadOnlyList<string> Roles);
