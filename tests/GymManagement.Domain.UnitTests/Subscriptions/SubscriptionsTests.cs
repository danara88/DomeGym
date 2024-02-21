using FluentAssertions;
using TestCommon.Gyms;
using TestCommon.Subscriptions;
using ErrorOr;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Domain.UnitTests.Subscriptions;

public class SubscriptionsTests
{
    /// <summary>
    /// Format explanation:
    /// AddGym: System under test
    /// WhenMoreThanSubscriptionAllows: Scenario that we are testing
    /// ShouldFail: Expected result
    /// </summary>
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        // Create a subscription
        var subscription = SubscriptionFactory.CreateSubscription();

        // Create the meximum number of gyms + 1
        var gyms = Enumerable.Range(0, subscription.GetMaxGyms() + 1)
                    .Select(_ => GymFactory.CreateGym(id: Guid.NewGuid()))
                    .ToList();

        // Act
        // Add all the various gyms
        var addGymResults = gyms.ConvertAll(subscription.AddGym);

        // Assert
        // Adding all the gyms succeeded, but the last failed

        // Save the last element
        var lastAddGymResult = addGymResults[addGymResults.Count - 1];

        // This removes the last element of the list
        addGymResults.RemoveAt(addGymResults.Count - 1);

        // Validate the results without the last element
        var allButLastGymResults = addGymResults;
        allButLastGymResults.Should().AllSatisfy(addGymResult => addGymResult.Value.Should().Be(Result.Success));

        // Validate the last element
        lastAddGymResult.IsError.Should().BeTrue();
        lastAddGymResult.FirstError.Should().Be(SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows);
    }
}
