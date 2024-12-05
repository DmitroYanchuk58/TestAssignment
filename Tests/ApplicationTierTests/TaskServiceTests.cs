using ApplicationTier.Services;
using ApplicationTier.Models;
using DataTier.Context;
using DataTier.Repositories;
using Microsoft.EntityFrameworkCore;
using DataTier.Entities;

using TaskDB = DataTier.Entities.Task;
using User = ApplicationTier.Models.User;
using Task = ApplicationTier.Models.Task;


namespace Tests.ApplicationTierTests
{
    public class TaskServiceTests
    {
        private User _user;
        private DataTier.Entities.User _userForCreating;
        private TaskRepository _taskRepository;
        private TaskService _service;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
            var context = new DatabaseContext(options);
            var userRepository = new UserRepository(context);
            _userForCreating = userRepository.ReadAll().Last();
            _user = new User(_userForCreating.Id, _userForCreating.Username, _userForCreating.Email, _userForCreating.PasswordHash, _userForCreating.Tasks);
            _taskRepository = new TaskRepository(context);
            _service = new TaskService(_taskRepository);
        }

        [Test]
        [Repeat(100)]
        public void CreateSuccess()
        {
            Task task = new Task()
            {
                Title = SharedClass.GetRandomString(20),
                Description = SharedClass.GetRandomString(100),
                Priority = Priority.High,
                DueDate = new DateTime(2024, 12, 3),
                Status = Status.InProgress,
            };


            Assert.DoesNotThrow(() => _service.Create(task,_user.Id));
        }

        [Test]
        public void CreateCountTasks()
        {
            var countBeforeCreating = _taskRepository.ReadAll().Count();

            Task task = new Task()
            {
                Title = SharedClass.GetRandomString(20),
                Description = SharedClass.GetRandomString(100),
                Priority = Priority.High,
                DueDate = new DateTime(2024, 12, 3),
                Status = Status.InProgress,
            };

            _service.Create(task, _user.Id);

            var countAfterCreating = _taskRepository.ReadAll().Count();

            Assert.IsTrue(countBeforeCreating + 1 == countAfterCreating);
        }

        [Test]
        public void CreateWihoutStatusAndPriority()
        {
            Task task = new Task()
            {
                Title = SharedClass.GetRandomString(20),
                Description = SharedClass.GetRandomString(100),
                DueDate = new DateTime(2024, 12, 3),
            };

            Assert.DoesNotThrow(() => _service.Create(task, _user.Id));
        }

        [Test]
        public void CreateWithoutTitle()
        {
            Task task = new Task()
            {
                Description = SharedClass.GetRandomString(100),
                Priority = Priority.High,
                DueDate = new DateTime(2024, 12, 3),
                Status = Status.InProgress,
            };

            Assert.Throws<Exception>(() => _service.Create(task, _user.Id));
        }

        [Test]
        public void CreateNull()
        {
            Task task = null;

            Assert.Throws<NullReferenceException>(() => _service.Create(task, _user.Id));
        }

        [Test]
        public void CreateEmpty()
        {
            Task task = new Task();

            Assert.Throws<Exception>(() => _service.Create(task, _user.Id));
        }

        [Test]
        public void UpdateSuccess()
        {
            TaskDB task = _taskRepository.ReadAll().Where(t => t.IdUser == _user.Id).Last();
            task.Description = SharedClass.GetRandomString(100);

            Assert.DoesNotThrow(() => _service.Update(task, _user.Id));
        }

        [Test]
        public void UpdateCheckUpdate()
        {
            var description = SharedClass.GetRandomString(100);
            TaskDB task = _taskRepository.ReadAll().Where(t => t.IdUser == _user.Id).Last();
            task.Description = description;
            _service.Update(task, _user.Id);

            Assert.IsTrue(task.Description.Equals(description));
        }

        [Test]
        public void UpdateWrongTask()
        {
            var task = CheckAllList();

            if (task != null)
            {
                Assert.Throws<UnauthorizedAccessException>(() => _service.Update(task, _user.Id));
            }
        }

        [Test]
        public void DeleteSuccess()
        {
            TaskDB task = _taskRepository.ReadAll().Where(t => t.IdUser == _user.Id).Last();

            Assert.DoesNotThrow(() => _service.Delete(task.Id, _user.Id));
        }

        [Test]
        public void DeleteWrongTask()
        {
            var task = CheckAllList();

            if (task != null)
            {
                Assert.Throws<UnauthorizedAccessException>(() => _service.Delete(task.Id, _user.Id));
            }
        }

        [Test]
        public void DeleteNotExistTask()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Get(Guid.NewGuid(), _user.Id));
        }

        [Test]
        public void ReadSuccess()
        {
            TaskDB task = _taskRepository.ReadAll().Where(t => t.IdUser == _user.Id).Last();

            Assert.DoesNotThrow(() => _service.Get(task.Id, _user.Id));
        }

        [Test]
        public void ReadWrongTask()
        {
            var task=CheckAllList();

            if (task != null)
            {

                Assert.Throws<UnauthorizedAccessException>(() => _service.Get(task.Id, _user.Id));
            }
        }

        [Test]
        public void ReadNotExistTask()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Get(Guid.NewGuid(), _user.Id));
        }

        private TaskDB CheckAllList()
        {
            var tasks = _taskRepository.ReadAll().ToList();
            int minCount = 0, maxCount = 100;
            bool condition = true;
            TaskDB task = null;

            while (condition)
            {
                var rangeSize = Math.Min(maxCount - minCount, tasks.Count - minCount);
                var tasksTemp = tasks.GetRange(minCount, rangeSize).Where(t => t.IdUser != _user.Id);

                if (tasksTemp.Any())
                {
                    task = tasksTemp.First();
                    condition = false;
                }
                else
                {
                    minCount = maxCount;
                    maxCount += 100;

                    if (minCount >= tasks.Count)
                    {
                        break;
                    }
                }
            }

            return task;
        }
    }
}
