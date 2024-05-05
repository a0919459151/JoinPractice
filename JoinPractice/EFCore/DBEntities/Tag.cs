namespace JoinPractice.EFCore.DBEntities;

public class Tag
{
    // Id
    public int Id { get; set; }

    // Name
    public required string Name { get; set; }


    // Posts
    public ICollection<Post>? Posts { get; set; } = [];

    // Post-Tag join table
    public ICollection<PostTag>? PostTags { get; set; } = [];
}
