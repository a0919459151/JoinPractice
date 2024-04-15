namespace JoinPractice.EFCore.DBEntities;

public class Blog
{
    // Id
    public int Id { get; set; }

    // Name
    public string Name { get; set; } = null!;

    // CreateAt
    public DateTime CreateAt { get; set; }

    // Posts
    public ICollection<Post>? Posts { get; set; }
}
