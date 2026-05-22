using Microsoft.EntityFrameworkCore;
using SmartDiary.Web.Data;
using SmartDiary.Web.Models;

namespace SmartDiary.Web.Services
{
	public class TaskService : ITaskService
	{
		private readonly ApplicationDbContext _context;

		public TaskService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async System.Threading.Tasks.Task<IEnumerable<TodoTask>>
			GetUserTasksAsync(string userId)
		{
			return await _context.Tasks
				.Where(t => t.UserId == userId)
				.ToListAsync();
		}

		public async System.Threading.Tasks.Task<TodoTask?>
			GetTaskByIdAsync(int id, string userId)
		{
			return await _context.Tasks
				.FirstOrDefaultAsync(t =>
					t.Id == id &&
					t.UserId == userId);
		}

		public async System.Threading.Tasks.Task<TodoTask>
			CreateTaskAsync(TodoTask task, string userId)
		{
			task.UserId = userId;

			_context.Tasks.Add(task);

			await _context.SaveChangesAsync();

			return task;
		}

		public async System.Threading.Tasks.Task<bool>
			DeleteTaskAsync(int id, string userId)
		{
			var task = await _context.Tasks
				.FirstOrDefaultAsync(t =>
					t.Id == id &&
					t.UserId == userId);

			if (task == null)
				return false;

			_context.Tasks.Remove(task);

			await _context.SaveChangesAsync();

			return true;
		}
	}
}