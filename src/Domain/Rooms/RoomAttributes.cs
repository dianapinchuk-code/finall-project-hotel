namespace Domain.Rooms;

public record RoomAttributes(int? Floor, int? Capacity, string? ViewType)
{private RoomAttributes() : this(null, null, null) { }

    public static RoomAttributes Empty() => new(null, null, null);
}