using GymManagement.Domain.Subscriptions;
using TestCommon.TestConstants;

namespace TestCommon.Subscriptions;

/// <summary>
/// Subscription Factory
/// </summary>
public static class SubscriptionFactory
{
    /// <summary>
    /// Method to create a subscription mock
    /// </summary>
    /// <param name="subscriptionType"></param>
    /// <param name="adminId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Subscription CreateSubscription(
        SubscriptionType? subscriptionType = null,
        Guid? adminId = null,
        Guid? id = null
    )
    {
        return new Subscription(
          subscriptionType: subscriptionType ?? Constants.Subscriptions.DefaultSubscriptionType,
          adminId ?? Constants.Admin.Id,
          id ?? Constants.Subscriptions.Id
        );
    }
}
