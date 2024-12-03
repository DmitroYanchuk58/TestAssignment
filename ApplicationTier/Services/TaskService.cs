using ApplicationTier.Interfaces;
using DataTier.Repositories;
using TaskDB = DataTier.Entities.Task;
using User = DataTier.Entities.User;
using Task = ApplicationTier.Models.Task;

namespace ApplicationTier.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskRepository _repository;
        private readonly User _user;

        public TaskService(TaskRepository repository,User user)
        {
            this._repository = repository;
            this._user= user;
        }

        public void Create(Models.Task newTask)
        {
            try
            {
                var taskDB=ConvertFromTaskToTaskDB(newTask);
                _repository.Create(taskDB);
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        public Task Get(Guid id)
        {
            try
            {
                var taskDB = _user.Tasks.Where(task => task.Id == id).First();
                if (taskDB.IdUser != _user.Id)
                {
                    throw new UnauthorizedAccessException("You don`t have permission to get this task.");
                }
                var task = ConvertFromTaskDBToTask(taskDB);
                return task;
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var task = this._repository.Read(id);
                if (task.IdUser != _user.Id)
                {
                    throw new KeyNotFoundException("Task wasn`t found.");
                }
                else if (task == null)
                {
                    throw new UnauthorizedAccessException("You don`t have permission to delete this task.");
                }
                else
                {
                    this._repository.Delete(id);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Task task)
        {
            try
            {
                var taskDB = _repository.Read(task.Id);
                if(taskDB.IdUser != this._user.Id)
                {
                    throw new UnauthorizedAccessException("You don`t have permission to update this task.");
                }
                this._repository.Update(taskDB);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TaskDB ConvertFromTaskToTaskDB(Task task)
        {
            TaskDB taskDB = new TaskDB()
            {
                Id = task.Id,
                Description = task.Description,
                Title = task.Title,
                CreatedAt = DateTime.Now,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                IdUser = _user.Id,
                User = _user
            };
            return taskDB;
        }

        private Task ConvertFromTaskDBToTask(TaskDB taskDB)
        {
            Task task = new Task()
            {
                Id = taskDB.Id,
                Description = taskDB.Description,
                Title = taskDB.Title,
                DueDate = taskDB.DueDate,
                Priority = taskDB.Priority,
                Status = taskDB.Status
            };
            return task;
        }
    }
}
