namespace UserProfileAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Friend
    {
        [Key]
        public Guid FriendId { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendsWith { get; set; }
    }
}
