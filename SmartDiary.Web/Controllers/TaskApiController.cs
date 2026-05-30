using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDiary.Web.Models;
using SmartDiary.Web.Services;
using System.Security.Claims;

namespace SmartDiary.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Только авторизованные пользователи
    public class TaskApiController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskApiController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/taskapi
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Пользователь не авторизован" });

            var tasks = await _taskService.GetUserTasksAsync(userId);
            return Ok(tasks);
        }

        // GET: api/taskapi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Пользователь не авторизован" });

            var task = await _taskService.GetTaskByIdAsync(id, userId);
            if (task == null)
                return NotFound(new { message = "Задача не найдена" });

            return Ok(task);
        }

        // POST: api/taskapi
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TodoTask task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Пользователь не авторизован" });

            var createdTask = await _taskService.CreateTaskAsync(task, userId);
            return Ok(createdTask);
        }

        // DELETE: api/taskapi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Пользователь не авторизован" });

            await _taskService.DeleteTaskAsync(id, userId);
            return Ok(new { message = "Задача удалена" });
        }
    }
}