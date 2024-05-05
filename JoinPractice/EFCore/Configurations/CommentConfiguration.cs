using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Contigurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Table
        builder.ToTable("Comments");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Property
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(100);

        // Navigation
        builder.HasOne(x => x.Post)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PostId);
    }
}
