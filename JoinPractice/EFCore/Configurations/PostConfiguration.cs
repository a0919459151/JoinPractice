using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Contigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.CreateAt).IsRequired();

        builder.HasOne(x => x.Blog).WithMany(x => x!.Posts).HasForeignKey(x => x.BlogId);

        // many-to-many
        builder
            .HasMany(e => e.PostTags)
            .WithMany(e => e.Posts);
            //.UsingEntity(
            //    "PostTag",
            //    l => l.HasOne(typeof(PostTag)).WithMany().HasForeignKey("TagId").HasPrincipalKey(nameof(PostTag.Id)),
            //    r => r.HasOne(typeof(Post)).WithMany().HasForeignKey("PostId").HasPrincipalKey(nameof(Post.Id)),
            //    j => j.HasKey("PostId", "TagId"));
    }
}
