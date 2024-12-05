using ApplicationTier.Services;
using DataTier.Entities;
using Task = ApplicationTier.Models.Task;
using TaskDB = DataTier.Entities.Task;

namespace ApplicationTier.Interfaces
{
    public interface ITaskService
    {
        public void Create(Task newTask, Guid userId);

        public Task Get(Guid id, Guid userId);

        public void Delete(Guid id, Guid userId);

        public void Update(TaskDB task, Guid userId);

        public List<Task> GetAllByPriority(Priority? priority, Guid userId);

        public List<Task> GetAll(Guid userId);

        public List<Task> GetAllByStatus(Status? status, Guid userId);

        public List<Task> GetTasksByDueDate(DateTime? dueDate, Guid userId);

        public List<Task> GetAllSort(SortOption? sortOption, Guid userId);

    }
}
