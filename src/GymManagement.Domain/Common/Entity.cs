namespace GymManagement.Domain.Common;

public abstract class Entity
{
  public Guid Id { get; init; }

  protected readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

  protected Entity(Guid id) => Id = id;

  protected Entity()
  {

  }

}
