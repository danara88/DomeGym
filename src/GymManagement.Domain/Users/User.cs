using ErrorOr;
using GymManagement.Domain.Common;
using GymManagement.Domain.Common.Interfaces;

namespace GymManagement.Domain.Users;

/// <summary>
/// User entity
/// </summary>
public class User : Entity
{
    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public string Email { get; } = null!;
    public Guid? AdminId { get; private set; }
    public Guid? ParticipantId { get; private set; }
    public Guid? TrainerId { get; private set; }

    private readonly string _passwordHash = null!;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="passwordHash"></param>
    /// <param name="adminId"></param>
    /// <param name="participantId"></param>
    /// <param name="trainerId"></param>
    /// <param name="id"></param>
    public User(
      string firstName,
      string lastName,
      string email,
      string passwordHash,
      Guid? adminId = null,
      Guid? participantId = null,
      Guid? trainerId = null,
      Guid? id = null
    ) : base(id ?? Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AdminId = adminId;
        ParticipantId = participantId;
        TrainerId = trainerId;
        _passwordHash = passwordHash;
    }

    /// <summary>
    /// Verrifies if the password is correct or not
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHasher"></param>
    /// <returns></returns>
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }

    /// <summary>
    /// Assigns an admin profile to the user
    /// </summary>
    /// <returns></returns>
    public ErrorOr<Guid> CreateAdminProfile()
    {
        if (AdminId is not null)
        {
            return Error.Conflict(description: "User already has an admin profile");
        }

        AdminId = Guid.NewGuid();

        return AdminId.Value;
    }

    /// <summary>
    /// Assigns a participant profile to the user
    /// </summary>
    /// <returns></returns>
    public ErrorOr<Guid> CreateParticiantProfile()
    {
        if (ParticipantId is not null)
        {
            return Error.Conflict(description: "User already has a participant profile");
        }

        ParticipantId = Guid.NewGuid();

        return ParticipantId.Value;
    }

    /// <summary>
    /// Assigns a trainer profile to the user
    /// </summary>
    /// <returns></returns>
    public ErrorOr<Guid> CreateTrainerProfile()
    {
        if (TrainerId is not null)
        {
            return Error.Conflict(description: "User already has a trainer profile");
        }

        TrainerId = Guid.NewGuid();

        return TrainerId.Value;
    }

    /// <summary>
    /// Gets all the profile types of the user
    /// </summary>
    /// <returns></returns>
    public List<ProfileType> GetProfileTypes()
    {
        List<ProfileType> profileTypes = new();

        if (AdminId is not null)
        {
            profileTypes.Add(ProfileType.Admin);
        }

        if (TrainerId is not null)
        {
            profileTypes.Add(ProfileType.Trainer);
        }

        if (ParticipantId is not null)
        {
            profileTypes.Add(ProfileType.Participant);
        }

        return profileTypes;
    }

    private User() {}
}
