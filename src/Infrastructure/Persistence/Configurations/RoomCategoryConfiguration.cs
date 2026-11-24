using Domain.Rooms;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoomCategoryConfiguration : IEntityTypeConfiguration<RoomCategory>
{
    public void Configure(EntityTypeBuilder<RoomCategory> builder)
    {
        builder.HasKey(rc => new { rc.RoomId, rc.CategoryId });

        builder.Property(rc => rc.RoomId).HasConversion<RoomIdConverter>();
        builder.Property(rc => rc.CategoryId).HasConversion<CategoryIdConverter>();

        builder.HasOne(rc => rc.Room)
            .WithMany(r => r.Categories)
            .HasForeignKey(rc => rc.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rc => rc.Category)
            .WithMany()
            .HasForeignKey(rc => rc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}