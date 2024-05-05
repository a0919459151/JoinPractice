namespace JoinPractice.EFCore.DBEntities;

public class Comment
{
    // Id
    public int Id { get; set; }

    // Content
    public required string Content { get; set; }

    // CreateAt
    public DateTime CreateAt { get; set; } = DateTime.Now;


    // Post
    public int? PostId { get; set; }
    public Post? Post { get; set; }
}
