using GymManagement.Domain.Gyms;
using TestCommon.TestConstants;

namespace TestCommon.Gyms;

/// <summary>
/// Gym Factory
/// </summary>
public static class GymFactory
{
    /// <summary>
    /// Method to create a gym mock
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxRooms"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Gym CreateGym(
        string name = Constants.Gym.Name,
        int maxRooms = Constants.Subscriptions.MaxRoomsFreeTier,
        Guid? id = null
    )
    {
        return new Gym(
          name,
          maxRooms,
          subscriptionId: Constants.Subscriptions.Id,
          id: id ?? Constants.Gym.Id
        );
    }
}
