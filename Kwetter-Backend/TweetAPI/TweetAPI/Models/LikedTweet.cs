namespace TweetAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LikedTweet
    {
        [Key]
        public Guid LikeId { get; set; }
        public string? UserId { get; set; }
        public Guid? TweetId { get; set; }
    }
}
