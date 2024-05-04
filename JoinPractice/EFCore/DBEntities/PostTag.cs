namespace JoinPractice.EFCore.DBEntities;

public class PostTag
{
    // Id
    public int Id { get; set; }

    // Name
    public string Name { get; set; } = null!;


    // Posts
    public ICollection<Post>? Posts { get; set; }
    
}
