using DataTier.Context;
using Task = DataTier.Entities.Task;

namespace DataTier.Repositories
{
    public class TaskRepository : IEntityRepository<Task>
    {

        private readonly DatabaseContext context;

        public TaskRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public Task Create(Task newEntity)
        {
                context.Tasks.Add(newEntity);
                context.SaveChanges();
                return newEntity;
        }

        public void Delete(Guid id)
        {
                var task = context.Tasks.Where(task => task.Id == id).First();
                context.Tasks.Remove(task);
                context.SaveChanges();
        }

        public Task Read(Guid id)
        {
                var task = context.Tasks.Where(task => task.Id == id).First();
                return task;
        }

        public bool Update(Task updatedTask)
        {
                if (string.IsNullOrEmpty(updatedTask.Title))
                {
                    throw new Exception();
                }

                var task = context.Tasks.Where(task => task.Id == updatedTask.Id).First();
                task = updatedTask;
                task.UpdatedAt= DateTime.Now;
                context.SaveChanges();
                return true;
        }

        public List<Task> ReadAll()
        {
             var tasks = context.Tasks.ToList();
             return tasks;
        }
    }
}
