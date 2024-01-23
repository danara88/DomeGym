using FluentValidation;

namespace GymManagement.Application.Rooms.Commands.CreateRoom;

/// <summary>
/// Create room command validator (model validation)
/// </summary>
public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.GymId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.RoomName)
            .MinimumLength(3)
            .MaximumLength(30);
    }
}
