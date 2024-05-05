using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Contigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Table
        builder.ToTable("Posts");

        // Key
        builder.HasKey(x => x.Id);

        // Property
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.CreateAt)
            .IsRequired();

        // Navigation
        builder.HasOne(x => x.Blog)
            .WithMany(x => x!.Posts)
            .HasForeignKey(x => x.BlogId);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<PostTag>();
                        
    }
}
