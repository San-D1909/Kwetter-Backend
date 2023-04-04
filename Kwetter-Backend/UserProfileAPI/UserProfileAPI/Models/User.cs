namespace UserProfileAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePic { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}
