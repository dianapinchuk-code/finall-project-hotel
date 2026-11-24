using Domain.Rooms;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
{
    public void Configure(EntityTypeBuilder<RoomImage> builder)
    {
        builder.HasKey(ri => ri.Id);
        builder.Property(ri => ri.Id).HasConversion<RoomImageIdConverter>();

        builder.Property(ri => ri.OriginalName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ri => ri.RoomId).HasConversion<RoomIdConverter>();

        builder.HasOne<Room>()
            .WithMany(r => r.Images)
            .HasForeignKey(ri => ri.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}