using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace JoinPractice.EFCore;

public class AppDbContext : DbContext
{
    // Blog
    public DbSet<Blog> Blogs { get; set; }
    // BlogHeader
    public DbSet<BlogHeader> BlogHeaders { get; set; }
    // Tag
    public DbSet<PostTag> PostTags { get; set; }
    // Tag
    public DbSet<Tag> Tags { get; set; }
    // Post
    public DbSet<Post> Posts { get; set; }
    // Comment
    public DbSet<Comment> Comments { get; set; }

    // On configuring
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=JoinPractice;User Id=sa;;Password=P@ssw0rdd;TrustServerCertificate=True;");
    }

    // On model creating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
