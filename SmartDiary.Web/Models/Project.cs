using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDiary.Web.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название проекта обязательно")]
        [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [RegularExpression("^([A-Fa-f0-9]{6})$", ErrorMessage = "Цвет должен быть в формате HEX (RRGGBB)")]
        public string Color { get; set; } = "808080";

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public string OwnerId { get; set; }

		[ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; } = null!;

        public ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
    }
}
