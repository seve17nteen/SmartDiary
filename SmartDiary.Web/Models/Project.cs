using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task = SmartDiary.Models.Task;  // ← ПСЕВДОНИМ

namespace SmartDiary.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [RegularExpression("^([A-Fa-f0-9]{6})$")]
        public string Color { get; set; } = "808080";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        public ICollection<Task> Tasks { get; set; } = new List<Task>();  // ← теперь Task — это твоя модель
    }
}