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
            _service = new TaskService(_taskRepository,_userForCreating);
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


            Assert.DoesNotThrow(()=>_service.Create(task));
        }

        [Test]
        public void CreateCountTasks()
        {
            var countBeforeCreating=_taskRepository.ReadAll().Count();

            Task task = new Task()
            {
                Title = SharedClass.GetRandomString(20),
                Description = SharedClass.GetRandomString(100),
                Priority = Priority.High,
                DueDate = new DateTime(2024, 12, 3),
                Status = Status.InProgress,
            };

            _service.Create(task);

            var countAfterCreating = _taskRepository.ReadAll().Count();

            Assert.IsTrue(countBeforeCreating+1 == countAfterCreating);
        }


    }
}
