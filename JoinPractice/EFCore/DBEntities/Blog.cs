namespace JoinPractice.EFCore.DBEntities;

public class Blog
{
    // Id
    public int Id { get; set; }

    // Name
    public required string Name { get; set; }

    // CreateAt
    public DateTime CreateAt { get; set; } = DateTime.Now;


    // BlogHeader
    public BlogHeader? BlogHeader { get; set; }

    // Posts
    public ICollection<Post>? Posts { get; set; } = [];
}
