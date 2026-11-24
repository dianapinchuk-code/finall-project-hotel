using Domain.Categories;
using Domain.Rooms;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters;

public class RoomIdConverter() : ValueConverter<RoomId, Guid>(
    id => id.Value,
    value => new RoomId(value));

public class CategoryIdConverter() : ValueConverter<CategoryId, Guid>(
    id => id.Value,
    value => new CategoryId(value));

public class RoomImageIdConverter() : ValueConverter<RoomImageId, Guid>(
    id => id.Value,
    value => new RoomImageId(value));