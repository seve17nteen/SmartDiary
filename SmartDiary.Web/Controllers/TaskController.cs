using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDiary.Web.Models;
using SmartDiary.Web.Services;
using System.Security.Claims;

namespace SmartDiary.Web.Controllers
{
	[Authorize]
	public class TaskController : Controller
	{
		private readonly ITaskService _taskService;

		public TaskController(ITaskService taskService)
		{
			_taskService = taskService;
		}

		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var tasks = await _taskService.GetUserTasksAsync(userId);

			return View(tasks);
		}

		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create(TodoTask todoTask)
        {
            if (!ModelState.IsValid)
            {
                return View(todoTask);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            await _taskService.CreateTaskAsync(todoTask, userId);

            return RedirectToAction(nameof(Index));
        }
    

		public async Task<IActionResult> Details(int id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var task = await _taskService.GetTaskByIdAsync(id, userId);

			if (task == null)
				return NotFound();

			return View(task);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			await _taskService.DeleteTaskAsync(id, userId);

			return RedirectToAction(nameof(Index));
		}
	}
}