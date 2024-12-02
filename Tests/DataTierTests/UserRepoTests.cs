using DataTier.Context;
using DataTier.Entities;
using DataTier.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Tests.DataTierTests
{
    public class UserRepoTests
    {
        private UserRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
            var context = new DatabaseContext(options);
            _repository = new UserRepository(context);
        }

        [Test]
        public void TestCreateSuccess()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = SharedClass.GetRandomString(6) + "@gmail.com",
                Username = SharedClass.GetRandomString(15),
                PasswordHash = SharedClass.GetRandomString(20) + "#@$",
                CreatedAt = DateTime.Now,
            };

            Assert.DoesNotThrow(() => _repository.Create(user));
        }

        [Test]
        public void TestCreateUsernameEmpty()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = SharedClass.GetRandomString(6) + "@gmail.com",
                Username = null,
                PasswordHash = SharedClass.GetRandomString(20),
                CreatedAt = DateTime.Now,
            };

            Assert.Throws<Exception>(() => _repository.Create(user));
        }

        [Test]
        public void TestCreateNull()
        {
            User userNull = null;

            Assert.Throws<Exception>(() => _repository.Create(userNull));
        }

        public void TestCreateEmpty()
        {
            User userNone = new User();

            Assert.Throws<Exception>(() => _repository.Create(userNone));
        }


        [Test]
        public void TestReadAll()
        {
            var users = _repository.ReadAll();
            Assert.DoesNotThrow(() => _repository.ReadAll());
            Assert.IsNotEmpty(users);
        }

        [Test]
        public void TestReadSuccess()
        {
            var user = _repository.ReadAll().First();

            var readUser = _repository.Read(user.Id);

            Assert.DoesNotThrow(() => _repository.Read(user.Id));
            Assert.AreEqual(user, readUser);
        }

        [Test]
        public void TestReadNotExistUser()
        {
            Assert.Throws<Exception>(() => _repository.Read(Guid.NewGuid()));
        }

        [Test]
        public void TestDelete()
        {
            var user= _repository.ReadAll().First();

            Assert.DoesNotThrow(()=>_repository.Delete(user.Id));
        }

        [Test]
        public void TestUpdate()
        {
            var user=_repository.ReadAll().First();
            user.Username=SharedClass.GetRandomString(10);
            Assert.DoesNotThrow(()=>_repository.Update(user));
            var conditionUpdate=_repository.Update(user);
            Assert.True(conditionUpdate);
        }
    }
}