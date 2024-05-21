using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Contigurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        // Table
        builder.ToTable("Blogs");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Property
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Navigation
        builder.HasOne(x => x.BlogHeader)
            .WithOne(x => x.Blog)
            .HasForeignKey<BlogHeader>(x => x.BlogId);
    }
}
