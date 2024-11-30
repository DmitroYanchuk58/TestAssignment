using DataTier.Context;
using DataTier.Entities;
using DataTier.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Tests
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
        public void TestCreate()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = SharedClass.GetRandomString(6)+"@gmail.com",
                Username = SharedClass.GetRandomString(15),
                PasswordHash = SharedClass.GetRandomString(20),
                CreatedAt = DateTime.Now,
            };

            var user2 = new User()
            {
                Id = Guid.NewGuid(),
                Email = SharedClass.GetRandomString(6) + "@gmail.com",
                Username = null,
                PasswordHash = SharedClass.GetRandomString(20),
                CreatedAt = DateTime.Now,
            };

            User userNull = null;

            User userNone = new User();

            Assert.DoesNotThrow(() => _repository.Create(user));
            Assert.Throws<Exception>(() => _repository.Create(user2));
            Assert.Throws<Exception>(() => _repository.Create(userNull));
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
        public void TestRead()
        {
            var user = _repository.ReadAll().First();

            var readUser = _repository.Read(user.Id);

            Assert.DoesNotThrow(() => _repository.Read(user.Id));
            Assert.Throws<Exception>(() => _repository.Read(Guid.NewGuid()));
            Assert.AreEqual(user, readUser);
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