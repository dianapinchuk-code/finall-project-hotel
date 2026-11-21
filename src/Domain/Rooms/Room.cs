namespace Domain.Rooms;

public class Room
{
    public RoomId Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }
    public RoomAttributes Attributes { get; private set; }

    private readonly List<RoomCategory> _categories = [];
    public IReadOnlyCollection<RoomCategory> Categories => _categories.AsReadOnly();

    private readonly List<RoomImage> _images = [];
    public IReadOnlyCollection<RoomImage> Images => _images.AsReadOnly();

    private Room(
        RoomId id,
        string title,
        string description,
        DateTime createdAt,
        DateTime? updatedAt,
        RoomAttributes attributes)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Attributes = attributes;
    }

    public static Room New(RoomId id, string title, string description, RoomAttributes? attributes = null)
        => new(id, title, description, DateTime.UtcNow, null, attributes ?? RoomAttributes.Empty());

    public void UpdateDetails(string title, string description, RoomAttributes? attributes = null)
    {
        Title = title;
        Description = description;
        Attributes = attributes ?? RoomAttributes.Empty();
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddCategory(RoomCategory link)
    {
        if (_categories.All(c => c.CategoryId != link.CategoryId))
        {
            _categories.Add(link);
        }
    }

    public void AddImage(RoomImage image)
    {
        _images.Add(image);
    }
}