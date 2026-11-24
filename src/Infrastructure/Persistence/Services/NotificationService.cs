using Application.Common.Interfaces.Services;
using Domain.Rooms;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class NotificationService(ILogger<NotificationService> logger) : INotificationService
{
    public Task SendRoomCreatedNotificationAsync(Room room, CancellationToken cancellationToken)
    {
        logger.LogInformation("Notification sent: Room '{Title}' ({RoomId}) has been created.", room.Title, room.Id);
        return Task.CompletedTask;
    }
}