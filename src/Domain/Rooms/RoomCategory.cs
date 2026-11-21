using Domain.Categories;

namespace Domain.Rooms;

public class RoomCategory
{
    public RoomId RoomId { get; private set; }
    public CategoryId CategoryId { get; private set; }

    public Room? Room { get; private set; }
    public Category? Category { get; private set; }

    private RoomCategory() { }

    public RoomCategory(RoomId roomId, CategoryId categoryId)
    {
        RoomId = roomId;
        CategoryId = categoryId;
    }
}