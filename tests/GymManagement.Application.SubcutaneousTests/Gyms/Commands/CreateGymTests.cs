using FluentAssertions;
using GymManagement.Application.SubcutaneousTests.Common;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Domain.Subscriptions;
using MediatR;
using TestCommon.Gyms;
using TestCommon.Subscriptions;

namespace GymManagement.Application.SubcutaneousTests.Gyms.Commands;

/// <summary>
/// Create gym tests (entire use case for creating a gym)
/// </summary>
[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateGymTests
{
    private readonly IMediator _mediator;

    public CreateGymTests(MediatorFactory mediatorFactory)
    {
        _mediator = mediatorFactory.CreateMediator();
    }

    [Fact]
    public async void CreateGym_WhenValidCommand_ShouldCreateGym()
    {
        // Arrange
        var subscription = await CreateSubscription();

        // Create a valid CreateGymCommand
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(subscriptionId: subscription.Id);

        // Act
        // Send the CreateGymCommand to MediatR
        var createGymResult = await _mediator.Send(createGymCommand);

        // Assert
        // The result is a gym corresponding to the details in the create gym command
        createGymResult.IsError.Should().BeFalse();
        createGymResult.Value.SubscriptionId.Should().Be(subscription.Id);
    }

    [Theory]
    [InlineData(0)]   // Test round 1 with 0 length
    [InlineData(1)]   // Test round 2 with 1 length
    [InlineData(200)] // Test round 3 with 200 length
    public async void CreateGym_WhenCommandContainsInvalidData_ShouldReturnValidationError(int gymNameLength)
    {
        // Arrange
        string gymName = new('a', gymNameLength);
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(name: gymName);

        // Act
        var result = await _mediator.Send(createGymCommand);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Name");
    }

    private async Task<Subscription> CreateSubscription()
    {
        //  1. Create a CreateSubscriptionCommand
        var createSubscriptionCommand = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();

        //  2. Sending it to MediatR
        var result = await _mediator.Send(createSubscriptionCommand);

        //  3. Making sure it was created successfully
        result.IsError.Should().BeFalse();
        return result.Value;
    }
}
