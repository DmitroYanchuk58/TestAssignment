using DataTier.Context;
using DataTier.Entities;
using DataTier.Repositories;
using Microsoft.EntityFrameworkCore;
using Task = DataTier.Entities.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Logging;

namespace Tests.DataTierTests
{
    public class TaskRepositoryTests
    {
        private TaskRepository _repository;
        private User _user;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
            var context = new DatabaseContext(options);
            _repository = new TaskRepository(context);
            var userRepository=new UserRepository(context);
            _user = userRepository.ReadAll().Last();
        }

        [Test]
        [Repeat(1000)]
        public void CreateSuccessfulAndTryCreateAgainTest()
        {

            Task task = new Task()
            {
                Title = SharedClass.GetRandomString(20),
                Description = SharedClass.GetRandomString(100),
                Priority = Priority.High,
                DueDate = new DateTime(2024, 12, 3),
                Status=Status.InProgress,
                IdUser=_user.Id,
                User=_user
            };

            Assert.DoesNotThrow(() => _repository.Create(task));
            Assert.Throws<Exception>(()=>_repository.Create(task));
        }

        [Test]
        public void CreateNullTest()
        {
            Task taskNull = null;

            Assert.Throws<Exception>(() => _repository.Create(taskNull));
        }

        [Test]
        public void CreateEmptyTest()
        {
            Task taskEmpty = new Task();

            Assert.Throws<Exception>(() => _repository.Create(taskEmpty));
        }

        [Test]
        public void CreateWithoutTitleTest()
        {
            Task taskWithoutTitle = new Task()
            {
                Description = SharedClass.GetRandomString(200),
                IdUser = _user.Id,
                User = _user,
                DueDate = new DateTime(2024, 12, 2)
            };

            Assert.Throws<Exception>(() => _repository.Create(taskWithoutTitle));
        }

        [Test]
        public void CreateWithoutUserTest()
        {
            Task taskWithoutUser = new Task()
            {
                Title = SharedClass.GetRandomString(20),
                Description = SharedClass.GetRandomString(200),
                DueDate = new DateTime(2024, 12, 2)
            };

            Assert.Throws<Exception>(() => _repository.Create(taskWithoutUser));
        }

        [Test]
        public void UpdateSuccessfulTest()
        {
            Task task = _repository.ReadAll().First();
            task.Description= SharedClass.GetRandomString(200);
            task.Title= SharedClass.GetRandomString(20);

            Assert.DoesNotThrow(() => _repository.Update(task));
        }

        [Test]
        public void UpdateEmptyTitleTest() {
            Task task = _repository.ReadAll().First();

            task.Title = string.Empty;

            Assert.IsFalse(_repository.Update(task));
        }

        [Test]
        public void UpdateDeleteUserTest()
        {
            Task task=_repository.ReadAll().First();

            task.User = null;
            task.IdUser = Guid.Empty;

            Assert.IsFalse(_repository.Update(task));
        }

        [Test]
        public void ReadAllTest()
        {
            var tasks =_repository.ReadAll();

            Assert.IsTrue(tasks.Any());
        }

        [Test]
        public void ReadSuccessTest() 
        { 
            var task = _repository.ReadAll().First();

            var readTask = _repository.Read(task.Id);

            Assert.IsNotNull(readTask);
        }

        [Test]
        public void ReadNotExistTaskTest()
        {
            Assert.Throws<Exception>(() => _repository.Read(Guid.NewGuid()));
        }

        [Test]
        public void DeleteTest()
        {
            var task=_repository.ReadAll().Last();

            Assert.DoesNotThrow(()=>_repository.Delete(task.Id));
        }

        [Test]
        public void DeleteNotExistTest()
        {
            Assert.Throws<Exception>(()=>_repository.Delete(Guid.NewGuid()));
        }
    }
}
