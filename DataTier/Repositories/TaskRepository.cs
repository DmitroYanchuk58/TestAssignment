using DataTier.Context;
using DataTier.Entities;
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
            try
            {
                context.Tasks.Add(newEntity);
                context.SaveChanges();
                return newEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var task = context.Tasks.Where(task => task.Id == id).First();
                context.Tasks.Remove(task);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task Read(Guid id)
        {
            try
            {
                var task = context.Tasks.Where(task => task.Id == id).First();
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Update(Task updatedTask)
        {
            try
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
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public List<Task> ReadAll()
        {
            try
            {
                var tasks = context.Tasks.ToList();
                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
