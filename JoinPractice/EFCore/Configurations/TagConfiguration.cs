using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Table
        builder.ToTable("Tags");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Property
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
