using DataTier.Context;
using DataTier.Repositories;
using Microsoft.EntityFrameworkCore;
using ApplicationTier.Services;
using ApplicationTier.Models;
using Moq;

namespace Tests.ApplicationTierTests
{
    public class AuthenticationServiceTests
    {
        private UserRepository _repository;
        private AuthenticationService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
            var context = new DatabaseContext(options);
            _repository = new UserRepository(context);
            _service = new AuthenticationService(_repository); 
        }

        [Test]
        public void TestRegister()
        {
            var user = new User(
                id: Guid.NewGuid(),
                email: SharedClass.GetRandomString(6) + "@gmail.com",
                username: SharedClass.GetRandomString(15),
                password: SharedClass.GetRandomString(20)
            );

            var userNull = new User(Guid.Empty, null, null, null);

            var userNull2 = new User(
                Guid.NewGuid(),
                null,
                SharedClass.GetRandomString(10) + "@email.com",
                SharedClass.GetRandomString(15));

            var existedUser = _repository.ReadAll().First();

            User existedUserForRegister = new User(existedUser.Id,
                existedUser.Username,
                existedUser.Email,
                existedUser.PasswordHash);

            Assert.DoesNotThrow(() => _service.Register(user));
            Assert.Throws<ArgumentNullException>(() => _service.Register(userNull));
            Assert.Throws<Exception>(() => _service.Register(userNull2));
            Assert.Throws<InvalidOperationException>(() => _service.Register(existedUserForRegister));
        }

        [Test]
        public void TestLogin()
        {
            var existedUser = new User(Guid.NewGuid(), "Dima58", "dimochka158@gmail.com", "990Tyeq12-9");
            Assert.DoesNotThrow(() => _service.Authenticate(existedUser.Username, existedUser.Password));
            Assert.IsTrue(_service.Authenticate(existedUser.Username, existedUser.Password));
            Assert.IsFalse(_service.Authenticate("aaaaa","bbbbb"));
        }

    }
}
