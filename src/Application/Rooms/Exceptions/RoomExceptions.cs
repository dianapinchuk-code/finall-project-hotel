using Domain.Categories;
using Domain.Rooms;

namespace Application.Rooms.Exceptions;

public abstract class RoomException(RoomId roomId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public RoomId RoomId { get; } = roomId;
}

public class RoomAlreadyExistsException(RoomId roomId)
    : RoomException(roomId, $"Room already exists under id {roomId}");

public class RoomNotFoundException(RoomId roomId)
    : RoomException(roomId, $"Room not found under id {roomId}");

public class RoomCategoriesNotFoundException(RoomId roomId, IReadOnlyCollection<CategoryId> missingCategoryIds)
    : RoomException(roomId, $"Categories not found: {string.Join(", ", missingCategoryIds)}")
{
    public IReadOnlyCollection<CategoryId> MissingCategoryIds { get; } = missingCategoryIds;
}

public class RoomUnhandledException(RoomId roomId, Exception? innerException = null)
    : RoomException(roomId, $"Unhandled exception under id {roomId}", innerException);