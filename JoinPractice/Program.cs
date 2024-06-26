﻿using JoinPractice.EFCore;
using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace JoinPractice;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var context = new AppDbContext();

        // Ensure Database is created, 
        // if not, it will create the database and run the migration
        await context.Database.EnsureCreatedAsync();

        // Data seeding
        await context.DataSeeding();

        // one-to-one relationship
        await BlogLeftJoinBlogHeader(context);
        await BlogInnerJoinBlogHeader(context);

        // one-to-many relationship
        await BlogLeftJoinPosts(context);
        await BlogInnerJoinPosts(context);

        // many-to-one relationship
        await PostLeftJoinBlog(context);
        await PostInnerJoinBlog(context);

        // many-to-many relationship
        await PostLeftJoinPostTags(context);
        await PostInnerJoinPostTags(context);
    }

    #region BlogLeftJoinBlogHeader
    private static async Task BlogLeftJoinBlogHeader(AppDbContext context)
    {
        await BlogLeftJoinBlogHeaderWithNavProp(context);

        await BlogLeftJoinBlogHeaderWithLinQ(context);

        await BlogLeftJoinBlogHeaderWithLambda(context);
    }

    private static async Task BlogLeftJoinBlogHeaderWithNavProp(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .Include(b => b.BlogHeader);

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[CreateAt], [b].[Name], [b0].[Id], [b0].[BlogId], [b0].[Title]
            FROM [Blogs] AS [b]
            LEFT JOIN [BlogHeaders] AS [b0] ON [b].[Id] = [b0].[BlogId]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogLeftJoinBlogHeaderWithLinQ(AppDbContext context)
    {
        // blog havs one blog header
        var blogQuery = from blog in context.Blogs
                        join blogHeader in context.BlogHeaders on blog.Id equals blogHeader.BlogId into blogHeaders
                        from blogHeader in blogHeaders.DefaultIfEmpty()
                        select new Blog
                        {
                            Id = blog.Id,
                            Name = blog.Name,
                            CreateAt = blog.CreateAt,
                            BlogHeader = blogHeader
                        };

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [b0].[Id], [b0].[BlogId], [b0].[Title]
            FROM [Blogs] AS [b]
            LEFT JOIN [BlogHeaders] AS [b0] ON [b].[Id] = [b0].[BlogId]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogLeftJoinBlogHeaderWithLambda(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .GroupJoin(
                context.BlogHeaders,
                blog => blog.Id,
                blogHeader => blogHeader.BlogId,
                (blog, blogHeaders) => new { blog, blogHeaders })
            .SelectMany(
                g => g.blogHeaders.DefaultIfEmpty(),
                (g, blogHeader) => new Blog
                {
                    Id = g.blog.Id,
                    Name = g.blog.Name,
                    CreateAt = g.blog.CreateAt,
                    BlogHeader = blogHeader
                });

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [b0].[Id], [b0].[BlogId], [b0].[Title]
            FROM [Blogs] AS [b]
            LEFT JOIN [BlogHeaders] AS [b0] ON [b].[Id] = [b0].[BlogId]
         */

        var blogs = await blogQuery.ToListAsync();
    }
    #endregion

    #region BlogInnerJoinBlogHeader
    private static async Task BlogInnerJoinBlogHeader(AppDbContext context)
    {
        await BlogInnerJoinBlogHeaderWithNavProp(context);

        await BlogInnerJoinBlogHeaderWithLinQ(context);

        await BlogInnerJoinBlogHeaderWithLambda(context);
    }

    private static async Task BlogInnerJoinBlogHeaderWithNavProp(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .Include(b => b.BlogHeader)
            .Where(b => b.BlogHeader != null);

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[CreateAt], [b].[Name], [b0].[Id], [b0].[BlogId], [b0].[Title]
            FROM [Blogs] AS [b]
            LEFT JOIN [BlogHeaders] AS [b0] ON [b].[Id] = [b0].[BlogId]
            WHERE [b0].[Id] IS NOT NULL
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogInnerJoinBlogHeaderWithLinQ(AppDbContext context)
    {
        var blogQuery = from blog in context.Blogs
                        join blogHeader in context.BlogHeaders on blog.Id equals blogHeader.BlogId
                        select new Blog
                        {
                            Id = blog.Id,
                            Name = blog.Name,
                            CreateAt = blog.CreateAt,
                            BlogHeader = blogHeader
                        };

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [b0].[Id], [b0].[BlogId], [b0].[Title]
            FROM [Blogs] AS [b]
            INNER JOIN [BlogHeaders] AS [b0] ON [b].[Id] = [b0].[BlogId]
         */

        var blogs = await blogQuery.ToListAsync();
    }

    private static async Task BlogInnerJoinBlogHeaderWithLambda(AppDbContext context)
    {
        var blogQuery = context.Blogs
            .Join(
                context.BlogHeaders,
                blog => blog.Id,
                blogHeader => blogHeader.BlogId,
                (blog, blogHeader) => new { blog, blogHeader })
            .Select(g => new Blog
            {
                Id = g.blog.Id,
                Name = g.blog.Name,
                CreateAt = g.blog.CreateAt,
                BlogHeader = g.blogHeader
            });

        var sql = blogQuery.ToQueryString();
        /*
            SELECT [b].[Id], [b].[Name], [b].[CreateAt], [b0].[Id], [b0].[BlogId], [b0].[Title]
            FROM [Blogs] AS [b]
            INNER JOIN [BlogHeaders] AS [b0] ON [b].[Id] = [b0].[BlogId]
         */

        var blogs = await blogQuery.ToListAsync();
    }
    #endregion

    #region BlogLeftJoinPosts
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
    #endregion

    #region BlogInnerJoinPosts
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
    #endregion

    #region PostInnerJoinBlog
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
    #endregion

    #region PostLeftJoinBlog
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
    #endregion

    #region PostLeftJoinPostTags
    private static async Task PostLeftJoinPostTags(AppDbContext context)
    {
        await PostLeftJoinPostTagsWithNavProp(context);

        await PostLeftJoinPostTagsWithLinQ(context);

        await PostLeftJoinPostTagsWithLambda(context);
    }

    private static async Task PostLeftJoinPostTagsWithNavProp(AppDbContext context)
    {
        // Directly use the navigation property
        var postQuery = context.Posts
            .Include(p => p.Tags);

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[BlogId], [p].[Content], [p].[CreateAt], [p].[Title], [t0].[PostId], [t0].[TagId], [t0].[CreateAt], [t0].[Id], [t0].[Name]
            FROM [Posts] AS [p]
            LEFT JOIN (
                SELECT [p0].[PostId], [p0].[TagId], [p0].[CreateAt], [t].[Id], [t].[Name]
                FROM [PostTags] AS [p0]
                INNER JOIN [Tags] AS [t] ON [p0].[TagId] = [t].[Id]
            ) AS [t0] ON [p].[Id] = [t0].[PostId]
            ORDER BY [p].[Id], [t0].[PostId], [t0].[TagId]
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostLeftJoinPostTagsWithLinQ(AppDbContext context)
    {
        // Start with the Posts table
        var postQuery = from post in context.Posts
                        select new Post
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CreateAt = post.CreateAt,
                            BlogId = post.BlogId,
                            Tags = (from postTag in context.PostTags
                                    where postTag.PostId == post.Id
                                    join tag in context.Tags on postTag.TagId equals tag.Id
                                    select tag)
                                    .ToList()
                        };

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [p].[BlogId], [t0].[Id], [t0].[Name], [t0].[PostId], [t0].[TagId]
            FROM [Posts] AS [p]
            LEFT JOIN (
                SELECT [t].[Id], [t].[Name], [p0].[PostId], [p0].[TagId]
                FROM [PostTags] AS [p0]
                INNER JOIN [Tags] AS [t] ON [p0].[TagId] = [t].[Id]
            ) AS [t0] ON [p].[Id] = [t0].[PostId]
            ORDER BY [p].[Id], [t0].[PostId], [t0].[TagId]
         */

        var p = await postQuery.ToListAsync();
    }

    private static async Task PostLeftJoinPostTagsWithLambda(AppDbContext context)
    {
        // Start with the Posts table
        var postQuery = context.Posts
            .Select(post => new Post
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreateAt = post.CreateAt,
                BlogId = post.BlogId,
                Tags = context.PostTags
                    .Where(postTag => postTag.PostId == post.Id)
                    .Join(context.Tags,
                          postTag => postTag.TagId,
                          tag => tag.Id,
                          (postTag, tag) => tag)
                    .ToList()
            });

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [p].[BlogId], [t0].[Id], [t0].[Name], [t0].[PostId], [t0].[TagId]
            FROM [Posts] AS [p]
            LEFT JOIN (
                SELECT [t].[Id], [t].[Name], [p0].[PostId], [p0].[TagId]
                FROM [PostTags] AS [p0]
                INNER JOIN [Tags] AS [t] ON [p0].[TagId] = [t].[Id]
            ) AS [t0] ON [p].[Id] = [t0].[PostId]
            ORDER BY [p].[Id], [t0].[PostId], [t0].[TagId]
         */

        var posts = await postQuery.ToListAsync();
    }
    #endregion

    #region PostInnerJoinPostTags
    private static async Task PostInnerJoinPostTags(AppDbContext context)
    {
        await PostInnerJoinPostTagsWithNavProp(context);

        await PostInnerJoinPostTagsWithLinQ(context);

        await PostInnerJoinPostTagsWithLambda(context);
    }

    private static async Task PostInnerJoinPostTagsWithNavProp(AppDbContext context)
    {
        // Use join table 
        var postQuery = context.Posts
            .Include(p => p.PostTags!)
            .ThenInclude(pt => pt.Tag)
            .Select(p => new Post
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreateAt = p.CreateAt,
                BlogId = p.BlogId,
                Tags = p.PostTags!.Select(pt => pt.Tag).ToList()
            });

        var sql = postQuery.ToQueryString();
        /*
            SELECT [p].[Id], [p].[Title], [p].[Content], [p].[CreateAt], [p].[BlogId], [t0].[Id], [t0].[Name], [t0].[PostId], [t0].[TagId]
            FROM [Posts] AS [p]
            LEFT JOIN (
                SELECT [t].[Id], [t].[Name], [p0].[PostId], [p0].[TagId]
                FROM [PostTags] AS [p0]
                INNER JOIN [Tags] AS [t] ON [p0].[TagId] = [t].[Id]
            ) AS [t0] ON [p].[Id] = [t0].[PostId]
            ORDER BY [p].[Id], [t0].[PostId], [t0].[TagId]
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostInnerJoinPostTagsWithLinQ(AppDbContext context)
    {
        // Start with the Join table
        var postQuery = from postTag in context.PostTags
                        join post in context.Posts on postTag.PostId equals post.Id
                        join tag in context.Tags on postTag.TagId equals tag.Id
                        group tag by post into groupedTags
                        select new Post
                        {
                            Id = groupedTags.Key.Id,
                            Title = groupedTags.Key.Title,
                            Content = groupedTags.Key.Content,
                            CreateAt = groupedTags.Key.CreateAt,
                            Tags = groupedTags.ToList()
                        };

        var sql = postQuery.ToQueryString();
        /*
            SELECT [t0].[Id], [t0].[Title], [t0].[Content], [t0].[CreateAt], [t0].[BlogId], [t1].[Id], [t1].[Name], [t1].[PostId], [t1].[TagId], [t1].[Id0]
            FROM (
                SELECT [p0].[Id], [p0].[Title], [p0].[Content], [p0].[CreateAt], [p0].[BlogId]
                FROM [PostTags] AS [p]
                INNER JOIN [Posts] AS [p0] ON [p].[PostId] = [p0].[Id]
                INNER JOIN [Tags] AS [t] ON [p].[TagId] = [t].[Id]
                GROUP BY [p0].[Id], [p0].[BlogId], [p0].[Content], [p0].[CreateAt], [p0].[Title]
            ) AS [t0]
            LEFT JOIN (
                SELECT [t2].[Id], [t2].[Name], [p1].[PostId], [p1].[TagId], [p2].[Id] AS [Id0], [p2].[BlogId], [p2].[Content], [p2].[CreateAt], [p2].[Title]
                FROM [PostTags] AS [p1]
                INNER JOIN [Posts] AS [p2] ON [p1].[PostId] = [p2].[Id]
                INNER JOIN [Tags] AS [t2] ON [p1].[TagId] = [t2].[Id]
            ) AS [t1] ON [t0].[Id] = [t1].[Id0] AND ([t0].[BlogId] = [t1].[BlogId] OR ([t0].[BlogId] IS NULL AND [t1].[BlogId] IS NULL)) AND [t0].[Content] = [t1].[Content] AND [t0].[CreateAt] = [t1].[CreateAt] AND [t0].[Title] = [t1].[Title]
            ORDER BY [t0].[Id], [t0].[BlogId], [t0].[Content], [t0].[CreateAt], [t0].[Title], [t1].[PostId], [t1].[TagId], [t1].[Id0]
         */

        var posts = await postQuery.ToListAsync();
    }

    private static async Task PostInnerJoinPostTagsWithLambda(AppDbContext context)
    {
        // Start with the Join table
        var postQuery = context.PostTags
            .Join(
                context.Posts,
                postTag => postTag.PostId,
                post => post.Id,
                (postTag, post) => new { postTag, post })
            .Join(
                context.Tags,
                p => p.postTag.TagId,
                tag => tag.Id,
                (p, tag) => new { p.post, tag })
            .GroupBy(p => p.post)
            .Select(groupedTags => new Post
            {
                Id = groupedTags.Key.Id,
                Title = groupedTags.Key.Title,
                Content = groupedTags.Key.Content,
                CreateAt = groupedTags.Key.CreateAt,
                Tags = groupedTags.Select(p => p.tag).ToList()
            });

        var sql = postQuery.ToQueryString();
        /*
            SELECT [t0].[Id], [t0].[Title], [t0].[Content], [t0].[CreateAt], [t0].[BlogId], [t1].[Id], [t1].[Name], [t1].[PostId], [t1].[TagId], [t1].[Id0]
            FROM (
                SELECT [p0].[Id], [p0].[Title], [p0].[Content], [p0].[CreateAt], [p0].[BlogId]
                FROM [PostTags] AS [p]
                INNER JOIN [Posts] AS [p0] ON [p].[PostId] = [p0].[Id]
                INNER JOIN [Tags] AS [t] ON [p].[TagId] = [t].[Id]
                GROUP BY [p0].[Id], [p0].[BlogId], [p0].[Content], [p0].[CreateAt], [p0].[Title]
            ) AS [t0]
            LEFT JOIN (
                SELECT [t2].[Id], [t2].[Name], [p1].[PostId], [p1].[TagId], [p2].[Id] AS [Id0], [p2].[BlogId], [p2].[Content], [p2].[CreateAt], [p2].[Title]
                FROM [PostTags] AS [p1]
                INNER JOIN [Posts] AS [p2] ON [p1].[PostId] = [p2].[Id]
                INNER JOIN [Tags] AS [t2] ON [p1].[TagId] = [t2].[Id]
            ) AS [t1] ON [t0].[Id] = [t1].[Id0] AND ([t0].[BlogId] = [t1].[BlogId] OR ([t0].[BlogId] IS NULL AND [t1].[BlogId] IS NULL)) AND [t0].[Content] = [t1].[Content] AND [t0].[CreateAt] = [t1].[CreateAt] AND [t0].[Title] = [t1].[Title]
            ORDER BY [t0].[Id], [t0].[BlogId], [t0].[Content], [t0].[CreateAt], [t0].[Title], [t1].[PostId], [t1].[TagId], [t1].[Id0]
         */

        var posts = await postQuery.ToListAsync();
    }
    #endregion
}
