namespace JoinPractice.EFCore.DBEntities;

public class BlogHeader
{
    // Id
    public int Id { get; set; }

    // Title
    public required string Title { get; set; }


    // Blog
    public int? BlogId { get; set; }
    public Blog? Blog { get; set;}
}
