namespace JoinPractice.EFCore.DBEntities;

public class Post
{
    // Id
    public int Id { get; set; }

    // Title
    public string Title { get; set; } = null!;

    // Content
    public string Content { get; set; } = null!;

    // CreateAt
    public DateTime CreateAt { get; set; }

    // Blog
    public int BlogId { get; set; }
    public Blog? Blog { get; set; }

    // Comments
    public ICollection<Comment>? Comments { get; set; }
}
