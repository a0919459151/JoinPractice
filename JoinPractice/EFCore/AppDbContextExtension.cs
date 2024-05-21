using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace JoinPractice.EFCore;

public static class AppDbContextExtension
{
    // Data Seeding
    public static async Task DataSeeding(this AppDbContext context)
    {
        // Begin transaction
        using var transaction = await context.Database.BeginTransactionAsync();

        // Init DB
        await context.InitDB();

        // Insert Tags
        await context.CreatePostTags();

        // Insert Tech Blog
        await context.CreateTechBlog();

        // Insert Cook BLog
        await context.CreateCookBlog();

        // Insert Blog without header
        await context.CreateNoHeaderBlog();

        // Insert Post
        await context.CreatePost(); 

        // Commit transaction
        await transaction.CommitAsync();
    }

    // Init Data
    private static async Task InitDB(this AppDbContext context)
    {
        context.Comments.RemoveRange(context.Comments);
        context.Posts.RemoveRange(context.Posts);
        context.Blogs.RemoveRange(context.Blogs);
        context.BlogHeaders.RemoveRange(context.BlogHeaders);
        context.Tags.RemoveRange(context.Tags);

        await context.SaveChangesAsync();
    }

    #region Data seed
    private static async Task CreatePostTags(this AppDbContext context)
    {
        List<Tag> postTags = [
            new Tag { Name = "C#" },
            new Tag { Name = "dotnet" },
            new Tag { Name = "EF Core" }
        ];

        await context.Tags.AddRangeAsync(postTags);

        await context.SaveChangesAsync();
    }

    private static async Task CreateTechBlog(this AppDbContext context)
    {
        var postTags = await context.Tags
            .ToListAsync();

        // BlogHeader
        var blogHeader = new BlogHeader
        {
            Title = "Tech Blog Header",
        };

        // Create a new blog
        var blog = new Blog
        {
            Name = "Tech Blog",
            CreateAt = DateTime.Now,
            BlogHeader = blogHeader
        };

        // Create multiple p
        var post1 = new Post
        {
            Title = "Introduction to C#",
            Content = "Content about C# basics",
            CreateAt = DateTime.Now,
            Comments = new List<Comment>
            {
                new Comment { Content = "Great post!", CreateAt = DateTime.Now },
                new Comment { Content = "Very helpful, thanks!", CreateAt = DateTime.Now }
            },
            Tags = postTags
                .Where(p => p.Name == "C#")
                .ToList()
        };

        var post2 = new Post
        {
            Title = "Advanced C#",
            Content = "Content about advanced C# topics",
            CreateAt = DateTime.Now,
            Comments = new List<Comment>
            {
                new Comment { Content = "I need more examples", CreateAt = DateTime.Now },
                new Comment { Content = "Can you cover async/await?", CreateAt = DateTime.Now }
            },
            Tags = postTags
                .Where(p => p.Name == "C#"
                    || p.Name == "dotnet")
                .ToList()
        };

        // Add p to the blog
        blog.Posts = [post1, post2];

        await context.Blogs.AddAsync(blog);

        await context.SaveChangesAsync();
    }

    private static async Task CreateCookBlog(this AppDbContext context)
    {
        // BlogHeader
        var blogHeader = new BlogHeader
        {
            Title = "Cook Blog Header",
        };

        // Create a new blog with no p
        var blog = new Blog
        {
            Name = "Cook blog",
            CreateAt = DateTime.Now,
            BlogHeader = blogHeader
        };

        await context.Blogs.AddAsync(blog);

        await context.SaveChangesAsync();
    }

    private static async Task CreateNoHeaderBlog(this AppDbContext context)
    {
        // Create a new blog with no header
        var blog = new Blog
        {
            Name = "No Header Blog",
            CreateAt = DateTime.Now
        };

        await context.Blogs.AddAsync(blog);

        await context.SaveChangesAsync();
    }

    private static async Task CreatePost(this AppDbContext context)
    {
        var postTags = await context.Tags
            .ToListAsync();

        // Create a new post without a blog
        var post = new Post
        {
            Title = "Entity Framework Core",
            Content = "Content about EF Core",
            CreateAt = DateTime.Now,
            Tags = postTags
        };

        await context.Posts.AddAsync(post);

        // Create a new post has no tag
        var post2 = new Post
        {
            Title = "Cooking",
            Content = "Content about Cooking",
            CreateAt = DateTime.Now
        };

        await context.Posts.AddAsync(post2);

        await context.SaveChangesAsync();
    }
    #endregion
}
