namespace GymManagement.Domain.Rooms;

/// <summary>
/// Room domain model
/// </summary>
public class Room
{
    public Guid Id { get; }
    public string Name { get; } = null!;

    public Guid GymId { get; }
    public int MaxDailySessions { get; }

    public Room(
        string name,
        Guid gymId,
        int maxDailySessions,
        Guid? id = null)
    {
        Name = name;
        GymId = gymId;
        MaxDailySessions = maxDailySessions;
        Id = id ?? Guid.NewGuid();
    }
}