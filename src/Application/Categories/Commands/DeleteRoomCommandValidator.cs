using FluentValidation;

namespace Application.Rooms.Commands;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();
    }
}