using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

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

    // On congiguring (Use local db)
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Server=localhost,1433;database=JoinPractice;User Id=sa;password=P@ssw0rdd;TrustServerCertificate=True;");
    //}

    // On congiguring (Use test container db)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string Image = "mcr.microsoft.com/mssql/server:2022-latest";
        const string Password = "P@ssw0rdd";
        const string Database = "JoinPractice";
        MsSqlContainer msSqlContainer = new MsSqlBuilder()
                .WithImage(Image)
                .WithPassword(Password)
                .Build();
        msSqlContainer.StartAsync().GetAwaiter().GetResult();
        optionsBuilder.UseSqlServer(GetConnectString());

        string GetConnectString()
        {
            var port = msSqlContainer.GetMappedPublicPort(1433);
            return $"Server=localhost,{port};database={Database};User Id=sa;password={Password};TrustServerCertificate=True;";
        }
    }

    // On model creating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
