namespace APW2.Service
{
    public interface ITaskManagerService
    {
        /// <summary>
        /// Asynchronously retrieves a TaskManager by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the TaskManager to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the requested <see cref="TaskManager"/>.</returns>
        Task<TaskManager> GetTaskManagerAsync(int id);
        /// <summary>
        /// Asynchronously retrieves all TaskManagers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing all <see cref="TaskManager"/>.</returns>
        Task<IEnumerable<TaskManager>> GetAllTaskManagersAsync();
        /// <summary>
        /// Asynchronously saves a new TaskManager into the database.
        /// </summary>
        /// <param name="TaskManager">The TaskManager to be saved.</param>
        /// <returns>A task that represents the asynchronous operation, containing all <see cref="TaskManager"/>.</returns>
        Task<TaskManager> SaveTaskManagerAsync(TaskManager taskManager);
        /// <summary>
        /// Asynchronously deletes a TaskManager from the database.
        /// </summary>
        /// <param name="TaskManager">The TaskManager to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation, containing all <see cref="TaskManager"/>.</returns>
        Task<bool> DeleteTaskManagerAsync(int id);
    }

    public class TaskManagerService : ITaskManagerService
    {
        private readonly ITaskManagerRepository _TaskManagerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManagerService"/> class.
        /// </summary>
        /// <param name="TaskManagerRepository">The repository used to access TaskManager data.</param>
        public TaskManagerService(ITaskManagerRepository taskManagerRepository)
        {
            _taskManagerRepository = taskManagerRepository;
        }

        public async Task<TaskManager> GetTaskManagerAsync(int id)
        {
            return await _taskManagerRepository.GetTaskManagerAsync(id);
        }

        public async Task<IEnumerable<TaskManager>> GetAllTaskManagersAsync()
        {
            return await _taskManagerRepository.GetAllTaskManagerAsync();
        }

        public async Task<TaskManager> SaveTaskManagerAsync(TaskManager taskManager)
        {
            return await _taskManagerRepository.SaveTaskManagerAsync(taskManager);
        }

        public async Task<bool> DeleteTaskManagerAsync(int id)
        {
            var taskManagers = await _taskManagerRepository.GetAllTaskManagerAsync();
            var deletion = taskManagers.SingleOrDefault(x => x.TaskManagerId == id);
            return await _taskManagerRepository.DeleteTaskManagerAsync(deletion);
        }
    }
}
