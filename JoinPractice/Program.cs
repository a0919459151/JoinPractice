using JoinPractice.EFCore;
using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace JoinPractice;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Init data
        // DataSeeding();

        using (var context = new AppDbContext())
        {
            // Left join, one-to-many relationship
            await BlogLeftJoinPosts(context);
            // Inner join, one-to-many relationship
            await BlogInnerJoinPosts(context);

        }
    }

    private static async Task BlogLeftJoinPosts(AppDbContext context)
    {
        await BlogLeftJoinPostsWithNavProp(context);

        await BlogLeftJoinPostsWithLinQ(context);

        await BlogLeftJoinPostsWithLambda(context);

        return;
    }

    private static async Task BlogLeftJoinPostsWithNavProp(AppDbContext context)
    {
        var blogQuery = context.Blogs
           .Include(b => b.Posts);

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[CreateAt], [b].[Name], [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title]
            FROM [Blogs] AS [b]
            LEFT JOIN [Posts] AS [p] ON [b].[Id] = [p].[BlogId]
            ORDER BY [b].[Id]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogLeftJoinPostsWithLinQ(AppDbContext context)
    {
        var blogQuery = from blog in context.Blogs
                        join post in context.Posts on blog.Id equals post.BlogId into posts
                        select new Blog
                        {
                            Id = blog.Id,
                            Name = blog.Name,
                            CreateAt = blog.CreateAt,
                            Posts = posts.ToList()
                        };

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [t].[Id], [t].[BlogId], [t].[Content], [t].[CreateAt], [t].[Title]
            FROM [Blogs] AS [b]
            OUTER APPLY (
                SELECT [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title]
                FROM [Posts] AS [p]
                WHERE [b].[Id] = [p].[BlogId]
            ) AS [t]
            ORDER BY [b].[Id]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogLeftJoinPostsWithLambda(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .GroupJoin(
            context.Posts,
            blog => blog.Id,
            post => post.BlogId,
            (blog, posts) => new { blog, posts })
            .Select(g => new Blog
            {
                Id = g.blog.Id,
                Name = g.blog.Name,
                CreateAt = g.blog.CreateAt,
                Posts = g.posts.ToList()
            });

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [t].[Id], [t].[BlogId], [t].[Content], [t].[CreateAt], [t].[Title]
            FROM [Blogs] AS [b]
            OUTER APPLY (
                SELECT [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title]
                FROM [Posts] AS [p]
                WHERE [b].[Id] = [p].[BlogId]
            ) AS [t]
            ORDER BY [b].[Id]
        */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogInnerJoinPosts(AppDbContext context)
    {
        await BlogInnerJoinPostsWithNavProp(context);

        await BlogInnerJoinPostsWithLinQ(context);

        await BlogInnerJoinPostsWithLambda(context);
    }



    private static async Task BlogInnerJoinPostsWithNavProp(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .Where(blog => blog.Posts!.Any())
            .Include(blog => blog.Posts);

        var sql = blogQuery.ToQueryString();
        /*
          SELECT [b].[Id], [b].[CreateAt], [b].[Name], [p0].[Id], [p0].[BlogId], [p0].[Content], [p0].[CreateAt], [p0].[Title]
          FROM [Blogs] AS [b]
          LEFT JOIN [Posts] AS [p0] ON [b].[Id] = [p0].[BlogId]
          WHERE EXISTS (
              SELECT 1
              FROM [Posts] AS [p]
              WHERE [b].[Id] = [p].[BlogId])
          ORDER BY [b].[Id]
       */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogInnerJoinPostsWithLinQ(AppDbContext context)
    {
        var blogQuery = from blog in context.Blogs
                        join post in context.Posts on blog.Id equals post.BlogId into posts
                        where posts.Any()
                        select new Blog()
                        {
                            Id = blog.Id,
                            Name = blog.Name,
                            CreateAt = blog.CreateAt,
                            Posts = posts.ToList()
                        };

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [t].[Id], [t].[BlogId], [t].[Content], [t].[CreateAt], [t].[Title]
            FROM [Blogs] AS [b]
            OUTER APPLY (
                SELECT [p0].[Id], [p0].[BlogId], [p0].[Content], [p0].[CreateAt], [p0].[Title]
                FROM [Posts] AS [p0]
                WHERE [b].[Id] = [p0].[BlogId]
            ) AS [t]
            WHERE EXISTS (
                SELECT 1
                FROM [Posts] AS [p]
                WHERE [b].[Id] = [p].[BlogId])
            ORDER BY [b].[Id]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogInnerJoinPostsWithLambda(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .Join(
            context.Posts,
            blog => blog.Id,
            post => post.BlogId,
            (blog, posts) => new { blog, posts })
            .GroupBy(bp => bp.blog)
            .Select(group => new Blog
            {
                Id = group.Key.Id,
                Name = group.Key.Name,
                CreateAt = group.Key.CreateAt,
                Posts = group.Select(bp => bp.posts).ToList()
            });

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [t].[Id], [t].[Name], [t].[CreateAt], [t0].[Id], [t0].[BlogId], [t0].[Content], [t0].[CreateAt], [t0].[Title], [t0].[Id0]
            FROM (
                SELECT [b].[Id], [b].[Name], [b].[CreateAt]
                FROM [Blogs] AS [b]
                INNER JOIN [Posts] AS [p] ON [b].[Id] = [p].[BlogId]
                GROUP BY [b].[Id], [b].[CreateAt], [b].[Name]
            ) AS [t]
            LEFT JOIN (
                SELECT [p0].[Id], [p0].[BlogId], [p0].[Content], [p0].[CreateAt], [p0].[Title], [b0].[Id] AS [Id0], [b0].[CreateAt] AS [CreateAt0], [b0].[Name]
                FROM [Blogs] AS [b0]
                INNER JOIN [Posts] AS [p0] ON [b0].[Id] = [p0].[BlogId]
            ) AS [t0] ON [t].[Id] = [t0].[Id0] AND [t].[CreateAt] = [t0].[CreateAt0] AND [t].[Name] = [t0].[Name]
            ORDER BY [t].[Id], [t].[CreateAt], [t].[Name], [t0].[Id0]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    #region Data seed
    private static async void DataSeeding()
    {
        using (var context = new AppDbContext())
        {
            // Init DB
            InitDB(context);

            context.Database.EnsureCreated();

            if (context.Blogs.Any()) return;

            await CreateTechBlog(context);  // Insert Tech Blog

            await CreateCookBlog(context);  // Insert Cook BLog

            context.SaveChanges();
        }
    }

    private static void InitDB(AppDbContext context)
    {
        context.Comments.RemoveRange(context.Comments);
        context.Posts.RemoveRange(context.Posts);
        context.Blogs.RemoveRange(context.Blogs);
    }

    private static async Task CreateTechBlog(AppDbContext context)
    {
        // Create a new blog
        var blog = new Blog { Name = "Tech Blog", CreateAt = DateTime.Now };

        // Create multiple posts
        var post1 = new Post
        {
            Title = "Introduction to C#",
            Content = "Content about C# basics",
            CreateAt = DateTime.Now,
            Comments = new List<Comment>
            {
                new Comment { Content = "Great post!", CreateAt = DateTime.Now },
                new Comment { Content = "Very helpful, thanks!", CreateAt = DateTime.Now }
            }
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
            }
        };

        // Add posts to the blog
        blog.Posts = [post1, post2];

        await context.Blogs.AddAsync(blog);
    }

    private static async Task CreateCookBlog(AppDbContext context)
    {
        // Create a new blog
        var blog = new Blog { Name = "Cook blog", CreateAt = DateTime.Now };

        await context.Blogs.AddAsync(blog);
    }
    #endregion
}
