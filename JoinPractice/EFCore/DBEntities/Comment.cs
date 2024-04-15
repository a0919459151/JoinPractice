namespace JoinPractice.EFCore.DBEntities;

public class Comment
{
    // Id
    public int Id { get; set; }

    // Content
    public string Content { get; set; } = null!;

    // CreateAt
    public DateTime CreateAt { get; set; }

    // Post
    public int PostId { get; set; }
    public Post? Post { get; set; }
}
