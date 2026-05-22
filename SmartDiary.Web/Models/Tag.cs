using System.ComponentModel.DataAnnotations;

namespace SmartDiary.Web.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тега обязательно")]
        [StringLength(50, ErrorMessage = "Название тега не может превышать 50 символов")]
        public string Name { get; set; } = string.Empty;

		public string OwnerId { get; set; }

		public User Owner { get; set; } = null!;

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
