using Domain.Rooms;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasConversion<RoomIdConverter>();

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(r => r.Title).IsUnique();

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.UpdatedAt);

        builder.OwnsOne(r => r.Attributes, ownedBuilder =>
        {
            ownedBuilder.ToJson();
        });

        builder.Navigation(r => r.Categories)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(r => r.Images)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}