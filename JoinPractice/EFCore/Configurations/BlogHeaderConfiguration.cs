using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Configurations;

public class BlogHeaderConfiguration : IEntityTypeConfiguration<BlogHeader>
{
    public void Configure(EntityTypeBuilder<BlogHeader> builder)
    {
        // Table
        builder.ToTable("BlogHeaders");

        // Primary Key
        builder.HasKey(bh => bh.Id);

        // Property
        builder.Property(bh => bh.Title)
            .IsRequired();
    }
}
