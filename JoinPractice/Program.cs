using JoinPractice.EFCore;
using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace JoinPractice;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var context = new AppDbContext();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Init data
        //await DataSeeding(context);


        // one-to-many relationship

        //await BlogLeftJoinPosts(context);
        //await BlogInnerJoinPosts(context);


        // many-to-one relationship

        //await PostLeftJoinBlog(context);
        //await PostInnerJoinBlog(context);



        // many-to-many relationship

        await PostLeftJoinPostTags(context);

    }

    private static async Task PostLeftJoinPostTags(AppDbContext context)
    {
        await PostLeftJoinPostTagsWithNavProp(context);

        await PostLeftJoinPostTagsWithLinQ(context);

        await PostLeftJoinPostTagsWithLambda(context);
    }

    private static async Task PostLeftJoinPostTagsWithNavProp(AppDbContext context)
    {
        var postQuery = context.Posts
            .Include(p => p.Tags);

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title], [t].[PostId], [t].[TagId], [t].[Id], [t].[Name]
            FROM [Posts] AS [p]
            LEFT JOIN (
                SELECT [p0].[PostId], [p0].[TagId], [p1].[Id], [p1].[Name]
                FROM [Tags] AS [p0]
                INNER JOIN [Tags] AS [p1] ON [p0].[TagId] = [p1].[Id]
            ) AS [t] ON [p].[Id] = [t].[PostId]
            ORDER BY [p].[Id], [t].[PostId], [t].[TagId]
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostLeftJoinPostTagsWithLinQ(AppDbContext context)
    {
        
    }

    private static async Task PostLeftJoinPostTagsWithLambda(AppDbContext context)
    {
        var postQuery = context.Posts
            .GroupJoin(
                context.Tags,
                post => post.Id,
                postTag => postTag.Id,
                (post, postTags) => new { post, postTags })
            .SelectMany(
                g => g.postTags.DefaultIfEmpty(),
                (g, postTag) => new Post
                {
                    Id = g.post.Id,
                    Title = g.post.Title,
                    Content = g.post.Content,
                    CreateAt = g.post.CreateAt,
                    BlogId = g.post.BlogId,
                    Tags = g.postTags.ToList()
                });

        var sql = postQuery.ToQueryString();

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostInnerJoinBlog(AppDbContext context)
    {
        await PostInnerJoinBlogWithNavProp(context);

        await PostInnerJoinBlogWithLinQ(context);

        await PostInnerJoinBlogWithLambda(context);
    }

    private static async Task PostInnerJoinBlogWithNavProp(AppDbContext context)
    {
        var postQuery = context.Posts
            .Include(p => p.Blog)
            .Where(p => p.Blog != null);

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title], [b].[Id], [b].[CreateAt], [b].[Name]
            FROM [Posts] AS [p]
            LEFT JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
            WHERE [b].[Id] IS NOT NULL
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostInnerJoinBlogWithLinQ(AppDbContext context)
    {
        var postQuery = from post in context.Posts
                        join blog in context.Blogs on post.BlogId equals blog.Id
                        select new Post
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CreateAt = post.CreateAt,
                            BlogId = post.BlogId,
                            Blog = blog
                        };

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [p].[BlogId], [b].[Id], [b].[CreateAt], [b].[Name]
            FROM [Posts] AS [p]
            INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostInnerJoinBlogWithLambda(AppDbContext context)
    {
        var postQuery = context.Posts
            .Join(
                context.Blogs,
                post => post.BlogId,
                blog => blog.Id,
                (post, blog) => new Post
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CreateAt = post.CreateAt,
                    BlogId = post.BlogId,
                    Blog = blog
                });

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [b].[Id], [b].[CreateAt], [b].[Name]
            FROM [Posts] AS [p]
            INNER JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostLeftJoinBlog(AppDbContext context)
    {
        await PostLeftJoinBlogWithNavProp(context);

        await PostLeftJoinBlogWithLinQ(context);

        await PostLeftJoinBlogWithLambda(context);
    }

    private static async Task PostLeftJoinBlogWithNavProp(AppDbContext context)
    {
        var postQuery = context.Posts
            .Include(p => p.Blog);

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title], [b].[Id], [b].[CreateAt], [b].[Name]
            FROM [Posts] AS [p]
            LEFT JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
        */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostLeftJoinBlogWithLinQ(AppDbContext context)
    {
        var postQuery = from post in context.Posts
                        join blog in context.Blogs on post.BlogId equals blog.Id into blogs
                        from blog in blogs.DefaultIfEmpty()
                        select new Post
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CreateAt = post.CreateAt,
                            BlogId = post.BlogId,
                            Blog = blog
                        };

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [p].[BlogId], [b].[Id], [b].[CreateAt], [b].[Name]
            FROM [Posts] AS [p]
            LEFT JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
        */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostLeftJoinBlogWithLambda(AppDbContext context)
    {
        var postQuery = context.Posts
            .GroupJoin(
                context.Blogs,
                post => post.BlogId,
                blog => blog.Id,
                (post, blogs) => new { post, blogs })
            .SelectMany(
                g => g.blogs.DefaultIfEmpty(),
                (g, blog) => new Post
                {
                    Id = g.post.Id,
                    Title = g.post.Title,
                    Content = g.post.Content,
                    CreateAt = g.post.CreateAt,
                    BlogId = g.post.BlogId,
                    Blog = blog
                });

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [p].[BlogId], [b].[Id], [b].[CreateAt], [b].[Name]
            FROM [Posts] AS [p]
            LEFT JOIN [Blogs] AS [b] ON [p].[BlogId] = [b].[Id]
        */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task BlogLeftJoinPosts(AppDbContext context)
    {
        await BlogLeftJoinPostsWithNavProp(context);

        await BlogLeftJoinPostsWithLinQ(context);

        await BlogLeftJoinPostsWithLambda(context);
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
    private static async Task DataSeeding(AppDbContext context)
    {
        // Begin transaction
        using var transaction = await context.Database.BeginTransactionAsync();

        // Init DB
        await InitDB(context);

        await CreatePostTags(context);  // Insert Tags

        await CreateTechBlog(context);  // Insert Tech Blog

        await CreateCookBlog(context);  // Insert Cook BLog

        await CreatePost(context);  // Insert Post

        // Commit transaction
        await transaction.CommitAsync();
    }

    private static async Task InitDB(AppDbContext context)
    {
        context.Comments.RemoveRange(context.Comments);
        context.Posts.RemoveRange(context.Posts);
        context.Blogs.RemoveRange(context.Blogs);
        context.Tags.RemoveRange(context.Tags);

        await context.SaveChangesAsync();
    }

    private static async Task CreatePostTags(AppDbContext context)
    {
        List<Tag> postTags = [
            new Tag { Name = "C#" },
            new Tag { Name = "dotnet" },
            new Tag { Name = "EF Core" }
        ];

        await context.Tags.AddRangeAsync(postTags);

        await context.SaveChangesAsync();
    }

    private static async Task CreateTechBlog(AppDbContext context)
    {
        var postTags = await context.Tags
            .ToListAsync();

        // Create a new blog
        var blog = new Blog
        {
            Name = "Tech Blog",
            CreateAt = DateTime.Now
        };

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

        // Add posts to the blog
        blog.Posts = [post1, post2];

        await context.Blogs.AddAsync(blog);

        await context.SaveChangesAsync();
    }

    private static async Task CreateCookBlog(AppDbContext context)
    {
        // Create a new blog with no posts
        var blog = new Blog { Name = "Cook blog", CreateAt = DateTime.Now };

        await context.Blogs.AddAsync(blog);

        await context.SaveChangesAsync();
    }

    private static async Task CreatePost(AppDbContext context)
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

        await context.SaveChangesAsync();
    }

    #endregion
}
