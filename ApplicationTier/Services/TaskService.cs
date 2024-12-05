using ApplicationTier.Interfaces;
using DataTier.Repositories;
using TaskDB = DataTier.Entities.Task;
using Task = ApplicationTier.Models.Task;
using DataTier.Entities;

namespace ApplicationTier.Services
{
    public enum SortOption
    {
        DueDateAsc,
        DueDateDesc,
        PriorityAsc,
        PriorityDesc
    }

    public class TaskService : ITaskService
    {
        private readonly TaskRepository _repository;

        public TaskService(TaskRepository repository)
        {
            _repository = repository;
        }

        public void Create(Models.Task newTask, Guid userId)
        {
            var taskDB = ConvertFromTaskToTaskDB(newTask, userId);
            _repository.Create(taskDB);
        }

        public Task Get(Guid id, Guid userId)
        {
            var taskDB = _repository.Read(id);

            if (taskDB == null || taskDB.IdUser != userId)
                throw new UnauthorizedAccessException("You don’t have permission to access this task.");

            return ConvertFromTaskDBToTask(taskDB);
        }

        public List<Task> GetAll(Guid userId)
        {
            var tasksDB = _repository.ReadAll().Where(t => t.IdUser == userId);
            return tasksDB.Select(ConvertFromTaskDBToTask).ToList();
        }

        public List<Task> GetAllByPriority(Priority? priority, Guid userId)
        {
            var tasksDB = _repository.ReadAll().Where(t => t.IdUser == userId && t.Priority == priority);
            return tasksDB.Select(ConvertFromTaskDBToTask).ToList();
        }

        public List<Task> GetAllByStatus(Status? status, Guid userId)
        {
            var tasksDB = _repository.ReadAll().Where(t => t.IdUser == userId && t.Status == status);
            return tasksDB.Select(ConvertFromTaskDBToTask).ToList();
        }

        public List<Task> GetTasksByDueDate(DateTime? dueDate, Guid userId)
        {
            var tasksDB = _repository.ReadAll().Where(t => t.IdUser == userId && t.DueDate == dueDate.Value.Date);

            return tasksDB.Select(ConvertFromTaskDBToTask).ToList();
        }
        public List<Task> GetAllSort(SortOption? sortOption, Guid userId)
        {
            var tasksDB = _repository.ReadAll().Where(t => t.IdUser == userId);

            tasksDB = sortOption switch
            {
                SortOption.DueDateAsc => tasksDB.OrderBy(t => t.DueDate),
                SortOption.DueDateDesc => tasksDB.OrderByDescending(t => t.DueDate),
                SortOption.PriorityAsc => tasksDB.OrderBy(t => t.Priority),
                SortOption.PriorityDesc => tasksDB.OrderByDescending(t => t.Priority),
                _ => tasksDB
            };

            return tasksDB.Select(ConvertFromTaskDBToTask).ToList();
        }

        public void Delete(Guid id, Guid userId)
        {
            var task = _repository.Read(id);
            if (task == null || task.IdUser != userId)
                throw new UnauthorizedAccessException("You don’t have permission to delete this task.");

            _repository.Delete(id);
        }

        public void Update(TaskDB task, Guid userId)
        {
            if (task.IdUser != userId)
                throw new UnauthorizedAccessException("You don’t have permission to update this task.");

            _repository.Update(task);
        }

        private static TaskDB ConvertFromTaskToTaskDB(Task task, Guid userId)
        {
            return new TaskDB
            {
                Id = task.Id,
                Description = task.Description,
                Title = task.Title,
                CreatedAt = DateTime.Now,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                IdUser = userId
            };
        }

        private static Task ConvertFromTaskDBToTask(TaskDB taskDB)
        {
            return new Task
            {
                Id = taskDB.Id,
                Description = taskDB.Description,
                Title = taskDB.Title,
                DueDate = taskDB.DueDate,
                Priority = taskDB.Priority,
                Status = taskDB.Status
            };
        }
    }
}
