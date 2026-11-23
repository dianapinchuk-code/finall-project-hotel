using FluentValidation;

namespace Application.Rooms.Commands;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(500);

        RuleFor(x => x.CategoryIds)
            .NotEmpty();

        RuleForEach(x => x.CategoryIds)
            .NotEmpty();
    }
}