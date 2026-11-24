using Domain.Categories;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion<CategoryIdConverter>();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.CreatedAt).IsRequired();
    }
}