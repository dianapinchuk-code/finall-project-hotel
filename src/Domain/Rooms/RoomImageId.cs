namespace Domain.Rooms;

public record RoomImageId(Guid Value)
{
    public static RoomImageId New() => new(Guid.NewGuid());
    public static RoomImageId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}