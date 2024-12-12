using APW2.Data.Models;
using APW2.Data.Repository;
using System.Collections.Concurrent;

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
        private readonly ITaskManagerRepository _taskManagerRepository;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ConcurrentDictionary<int, CancellationTokenSource> _taskCancellationTokens = new ConcurrentDictionary<int, CancellationTokenSource>();

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
            var deletion = taskManagers.SingleOrDefault(x => x.TaskId == id);
            return await _taskManagerRepository.DeleteTaskManagerAsync(deletion);
        }

        public async Task ExecuteTasksAsync()
        {
            await _semaphore.WaitAsync(); 
            try
            {
                var tasks = await _taskManagerRepository.GetAllTaskManagerAsync(); 
                foreach (var task in tasks.Where(t => t.Status == "Pending"))
                {
                    var cancellationTokenSource = new CancellationTokenSource(); 
                    _taskCancellationTokens[task.TaskId] = cancellationTokenSource; 
                    var command = new TaskCommand(task, _taskManagerRepository); 
                    await command.ExecuteAsync(cancellationTokenSource.Token); 
                    await Task.Delay(5000); // Espera de 5 segundos entre tareas
                } 
            }
            finally 
            { 
                _semaphore.Release();
            } 
        }
        public async Task StopTaskAsync(int taskId) 
        { 
            if (_taskCancellationTokens.TryGetValue(taskId, out var cancellationTokenSource)) 
            { 
                cancellationTokenSource.Cancel(); 
                _taskCancellationTokens.TryRemove(taskId, out _); 
            } 
        }
    }
    public interface ICommand 
    { Task ExecuteAsync(CancellationToken cancellationToken); 
    }
    public class TaskCommand : ICommand
    {
        private readonly TaskManager _taskManager; 
        private readonly ITaskManagerRepository _taskManagerRepository; 
        public TaskCommand(TaskManager taskManager, ITaskManagerRepository taskManagerRepository) 
        { _taskManager = taskManager; _taskManagerRepository = taskManagerRepository; 
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _taskManager.Status = "In Progress"; await _taskManagerRepository.SaveTaskManagerAsync(_taskManager); 
                await Task.Delay(10000, cancellationToken); // Simula una operación de 10 segundos
                
                _taskManager.Status = "Completed"; 
                await _taskManagerRepository.SaveTaskManagerAsync(_taskManager); 
            } 
            catch (OperationCanceledException) 
            { 
                _taskManager.Status = "Cancelled"; 
                await _taskManagerRepository.SaveTaskManagerAsync(_taskManager); 
            } 
        } 
    } 
}
