namespace UserProfileAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Friend
    {
        [Key]
        public Guid FriendId { get; set; }
        public string? UserId { get; set; }
        public string? FriendsWith { get; set; }
    }
}
