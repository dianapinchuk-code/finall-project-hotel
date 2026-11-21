namespace Domain.Rooms;

public record RoomAttributes(int? Floor, int? Capacity, string? ViewType)
{
    public static RoomAttributes Empty() => new(null, null, null);
}