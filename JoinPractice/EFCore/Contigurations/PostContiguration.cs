using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoinPractice.EFCore.Contigurations;

public class PostContiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.CreateAt).IsRequired();

        builder.HasOne(x => x.Blog).WithMany(x => x!.Posts).HasForeignKey(x => x.BlogId);
    }
}
