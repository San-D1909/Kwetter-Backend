namespace TweetAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }
        public Guid? UserId { get; set; }
        public string? Body { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
