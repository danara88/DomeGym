using FluentValidation;

namespace GymManagement.Application.Rooms.Commands.DeleteRoom;

/// <summary>
/// Delete room command validator (model validation)
/// </summary>
public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(x => x.GymId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.RoomId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
