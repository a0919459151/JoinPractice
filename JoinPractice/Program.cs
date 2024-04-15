using JoinPractice.EFCore;
using JoinPractice.EFCore.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace JoinPractice;

internal class Program
{
    static void Main(string[] args)
    {
        // Init Post data
        InitPostData();

        using (var context = new AppDbContext())
        {
            JoinPractice(context);
        }
    }

    private static void JoinPractice(AppDbContext context)
    {
        var blog = context.Blogs
            .Include(b => b.Posts)
            .ToListAsync();

        return;
    }

    private static void InitPostData()
    {
        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();

            if (context.Blogs.Any()) return;

            var blog =  CreateBlogEntity();

            context.Blogs.Add(blog);

            context.SaveChanges();
        }
    }

    private static Blog CreateBlogEntity()
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

        return blog;
    }
}
