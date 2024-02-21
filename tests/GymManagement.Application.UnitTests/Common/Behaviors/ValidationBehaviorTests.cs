using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Domain.Gyms;
using MediatR;
using NSubstitute;
using TestCommon.Gyms;

namespace GymManagement.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly ValidationBehavior<CreateGymCommand, ErrorOr<Gym>> _validationBehavior;
    private readonly IValidator<CreateGymCommand> _mockValidator;
    private readonly RequestHandlerDelegate<ErrorOr<Gym>> _mockNextBehavior;

    public ValidationBehaviorTests()
    {
        // Create next behavior mock
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();

        // Create validator mock
        _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();

        /// Create validation behavior (SUT = System Under Test)
        _validationBehavior = new ValidationBehavior<CreateGymCommand, ErrorOr<Gym>>(_mockValidator);
    }

    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        // Create a request
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();

        _mockValidator
            .ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        // Create a next behavior (mock)
        var gym = GymFactory.CreateGym();
        _mockNextBehavior.Invoke().Returns(gym);

        // Act
        // Invoke behavior
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, default);

        // Assert
        // Result from invoking the behavior, was the result returned by the next behavior
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }

    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsNotValid_ShouldReturnListOfErrors()
    {
        // Arrange
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();
        var validationFailures = new List<ValidationFailure>
        {
            new (propertyName: "foo", errorMessage: "bad foo")
        };

        _mockValidator
            .ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        // Act
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("foo");
        result.FirstError.Description.Should().Be("bad foo");
    }
}
