namespace TweetAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Tweet
    {
        [Key]
        public Guid TweetId { get; set; }
        public Guid? UserId { get; set; }
        public string? Header { get; set; }
        public string? Body { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsArchived { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
