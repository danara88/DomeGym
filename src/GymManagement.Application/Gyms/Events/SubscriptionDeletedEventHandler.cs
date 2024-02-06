using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using MediatR;

namespace GymManagement.Application.Gyms.Events;

/// <summary>
/// Subscription deleted event handler.
/// This event handler will deletes all th gyms related to the deleted subscription.
/// </summary>
public class SubscriptionDeletedEventHandler : INotificationHandler<SubscriptionDeletedEvent>
{
    private readonly IGymsRepository _gymRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionDeletedEventHandler(IGymsRepository gymsRepository, IUnitOfWork unitOfWork)
    {
        _gymRepository = gymsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var gyms = await _gymRepository.ListBySubscriptionIdAsync(notification.SubscriptionId);

        await _gymRepository.RemoveRangeAsync(gyms);

        await _unitOfWork.CommitChangesAsync();
    }
}
