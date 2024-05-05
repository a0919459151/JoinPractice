namespace JoinPractice.EFCore.DBEntities;

public class PostTag
{
    // CreateAt
    public DateTime CreateAt { get; set; } = DateTime.Now;


    // PostId
    public int PostId { get; set; }
    public required Post Post { get; set; }

    // TagId
    public int TagId { get; set; }
    public required Tag Tag { get; set; }
}
