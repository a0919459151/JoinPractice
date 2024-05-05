namespace JoinPractice.EFCore.DBEntities;

public class Post
{
    // Id
    public int Id { get; set; }

    // Title
    public required string Title { get; set; }

    // Content
    public required string Content { get; set; }

    // CreateAt
    public DateTime CreateAt { get; set; }


    // Blog
    public int? BlogId { get; set; }
    public Blog? Blog { get; set; }

    // Post-Tag join table
    public ICollection<PostTag>? PostTags { get; set; } = [];

    // Tags
    public ICollection<Tag>? Tags { get; set; } = [];

    // Comments
    public ICollection<Comment>? Comments { get; set; } = [];
}
