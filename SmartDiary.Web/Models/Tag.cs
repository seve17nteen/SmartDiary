using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDiary.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}