using System.ComponentModel.DataAnnotations;
using Task = SmartDiary.Models.Task;  // ← ПСЕВДОНИМ

namespace SmartDiary.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Settings { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();  // ← теперь Task — это твоя модель
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}