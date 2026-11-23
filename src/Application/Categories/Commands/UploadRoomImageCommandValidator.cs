using FluentValidation;

namespace Application.Rooms.Commands;

public class UploadRoomImageCommandValidator : AbstractValidator<UploadRoomImageCommand>
{
    public UploadRoomImageCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty();

        RuleFor(x => x.OriginalName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Content)
            .NotNull();
    }
}