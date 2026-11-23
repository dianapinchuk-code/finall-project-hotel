using Domain.Rooms;

namespace Application.Common.Interfaces.Services;

public interface INotificationService
{
    Task SendRoomCreatedNotificationAsync(Room room, CancellationToken cancellationToken);
}