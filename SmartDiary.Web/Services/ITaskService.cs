using SmartDiary.Web.Models;

namespace SmartDiary.Web.Services
{
	public interface ITaskService
	{
		System.Threading.Tasks.Task<IEnumerable<TodoTask>>
			GetUserTasksAsync(string userId);

		System.Threading.Tasks.Task<TodoTask?>
			GetTaskByIdAsync(int id, string userId);

		System.Threading.Tasks.Task<TodoTask>
			CreateTaskAsync(TodoTask task, string userId);

		System.Threading.Tasks.Task<bool>
			DeleteTaskAsync(int id, string userId);
	}
}