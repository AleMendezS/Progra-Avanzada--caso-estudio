using Microsoft.EntityFrameworkCore;
using APW2.Data.Models;

namespace AB.Data.Repository;

/// <summary>
/// Interface for TaskManager repository operations.
/// </summary>
public interface ITaskManagerRepository
{
	/// <summary>
	/// Deletes a TaskManager asynchronously.
	/// </summary>
	/// <param name="TaskManager">The TaskManager to delete.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
	Task<bool> DeleteTaskManagerAsync(TaskManager taskManager);

    /// <summary>
    /// Retrieves a TaskManager by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the TaskManager.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TaskManager if found; otherwise, null.</returns>
    Task<TaskManager> GetTaskManagerAsync(int id);

    Task<IEnumerable<TaskManager>> GetAllTaskManagerAsync();

    /// <summary>
    /// Saves a collection of TaskManager categories asynchronously.
    /// </summary>
    /// <param name="categories">The collection of TaskManagers to save.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> SaveCategoriesAsync(IEnumerable<TaskManager> categories);

    /// <summary>
    /// Saves a TaskManager asynchronously. If the TaskManager exists, it updates; otherwise, it creates a new one.
    /// </summary>
    /// <param name="TaskManager">The TaskManager to save.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<TaskManager> SaveTaskManagerAsync(TaskManager taskManager);

    
}

/// <summary>
/// Implementation of the TaskManager repository interface.
/// </summary>
public class TaskManagerRepository : RepositoryBase<TaskManager>, ITaskManagerRepository
{
    /// <summary>
    /// Saves a TaskManager asynchronously. If the TaskManager exists, it updates it; otherwise, it creates a new one.
    /// </summary>
    /// <param name="TaskManager">The TaskManager to save.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<TaskManager> SaveTaskManagerAsync(TaskManager taskManager)
    {
        var exists = taskManager.TaskId != null && taskManager.TaskId > 0;
        if (exists)
            await UpdateAsync(taskManager);
        else
            await CreateAsync(taskManager);
        var updated = await ReadAsync();
        return updated.SingleOrDefault(x => x.TaskId == taskManager.TaskId)!;
    }

    /// <summary>
    /// Saves a collection of TaskManager categories asynchronously. Each category is either updated or created.
    /// </summary>
    /// <param name="categories">The collection of TaskManagers to save.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    public async Task<bool> SaveCategoriesAsync(IEnumerable<TaskManager> categories)
	{
		foreach (var c in categories)
		{
			var exists = await ExistsAsync(c);
			if (exists)
				await UpdateAsync(c);
			else
				await CreateAsync(c);
		}

		return true;
	}

	/// <summary>
	/// Deletes a TaskManager asynchronously.
	/// </summary>
	/// <param name="TaskManager">The TaskManager to delete.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
	public async Task<bool> DeleteTaskManagerAsync(TaskManager taskManager)
	{
		return await DeleteAsync(taskManager);
	}

    /// <summary>
    /// Retrieves a TaskManager by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the TaskManager.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TaskManager if found; otherwise, null.</returns>
    public async Task<TaskManager> GetTaskManagerAsync(int id)
    {
        var taskManagers = await ReadAsync();
        return taskManagers.SingleOrDefault(x => x.TaskId == id)!;
    }

    /// <summary>
    /// Retrieves all products asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing all products.</returns>
    public async Task<IEnumerable<TaskManager>> GetAllTaskManagerAsync()
    {
        return await ReadAsync();
    }
}
