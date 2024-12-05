using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
