using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDiary.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? Deadline { get; set; }

        [Required]
        public string Status { get; set; } = "New";

        [Required]
        public string Priority { get; set; } = "Medium";

        public int? ProjectId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}