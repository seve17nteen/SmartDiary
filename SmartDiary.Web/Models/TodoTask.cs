using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDiary.Web.Models
{
    public class TodoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок задачи обязателен")]
        [StringLength(200, ErrorMessage = "Заголовок не может превышать 200 символов")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(TodoTask), nameof(ValidateDeadline))]
        public DateTime? Deadline { get; set; }

        [Required]
        [RegularExpression("^(New|InProgress|Completed)$", ErrorMessage = "Статус должен быть New, InProgress или Completed")]
        public string Status { get; set; } = "New";

        [Required]
        [RegularExpression("^(Low|Medium|High)$", ErrorMessage = "Приоритет должен быть Low, Medium или High")]
        public string Priority { get; set; } = "Medium";

        public int? ProjectId { get; set; }

        public string? UserId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();

        public static ValidationResult? ValidateDeadline(DateTime? deadline, ValidationContext context)
        {
            if (deadline.HasValue && deadline.Value.Date < DateTime.UtcNow.Date)
            {
                return new ValidationResult("Дедлайн не может быть в прошлом");
            }

            return ValidationResult.Success;
        }
    }
}
