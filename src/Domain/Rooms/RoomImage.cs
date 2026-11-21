namespace Domain.Rooms;

public class RoomImage
{
    public RoomImageId Id { get; }
    public string OriginalName { get; private set; }
    public RoomId RoomId { get; private set; }

    private RoomImage(RoomImageId id, string originalName, RoomId roomId)
    {
        Id = id;
        OriginalName = originalName;
        RoomId = roomId;
    }

    public static RoomImage New(RoomImageId id, string originalName, RoomId roomId)
        => new(id, originalName, roomId);
}