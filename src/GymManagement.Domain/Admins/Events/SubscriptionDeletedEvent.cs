using GymManagement.Domain.Common;

namespace GymManagement.Domain.Admins.Events;

/// <summary>
/// Subscription deleted event
/// </summary>
/// <param name="SubscriptionId"></param>
public record SubscriptionDeletedEvent(Guid SubscriptionId) : IDomainEvent;
