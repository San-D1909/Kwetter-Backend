namespace TweetAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }
        public string? UserId { get; set; }
        public string? Body { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
