using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTier.Entities;
using Task = ApplicationTier.Models.Task;

namespace ApplicationTier.Interfaces
{
    public interface ITaskService
    {
        public void Create(Task newTask);

        public Task Get(Guid id);

        public void Delete(Guid id);

        public void Update(Task task);
    }
}
