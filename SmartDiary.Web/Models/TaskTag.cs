using Microsoft.EntityFrameworkCore;
using Task = SmartDiary.Models.Task;  // ← ПСЕВДОНИМ

namespace SmartDiary.Models
{
    [PrimaryKey(nameof(TaskId), nameof(TagId))]
    public class TaskTag
    {
        public int TaskId { get; set; }
        public int TagId { get; set; }

        public Task Task { get; set; } = null!;  // ← теперь Task — это твоя модель
        public Tag Tag { get; set; } = null!;
    }
}