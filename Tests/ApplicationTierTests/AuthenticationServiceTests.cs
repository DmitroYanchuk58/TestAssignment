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
        public void TestRegisterSuccess()
        {
            var user = new User(
                id: Guid.NewGuid(),
                email: SharedClass.GetRandomString(6) + "@gmail.com",
                username: SharedClass.GetRandomString(15),
                password: SharedClass.GetRandomString(20),
                tasks: null
            );

            Assert.DoesNotThrow(() => _service.Register(user));
        }

        [Test]
        public void TestRegisterNull()
        {
            var userNull = new User(Guid.Empty, null, null, null, null);

            Assert.Throws<ArgumentNullException>(() => _service.Register(userNull));
        }

        [Test]
        public void TestRegisterWithoutEmail()
        {
            var user = new User(
                Guid.NewGuid(),
                null,
                SharedClass.GetRandomString(10) + "@email.com",
                SharedClass.GetRandomString(15),
                null);

            Assert.Throws<Exception>(() => _service.Register(user));
        }

        [Test]
        public void TestRegisterExistUser()
        {

            var existedUser = _repository.ReadAll().First();

            User existedUserForRegister = new User(existedUser.Id,
                existedUser.Username,
                existedUser.Email,
                existedUser.PasswordHash,
                null);

            Assert.Throws<InvalidOperationException>(() => _service.Register(existedUserForRegister));
        }

        [Test]
        public void TestLoginSuccess()
        {
            var existedUser = new User(Guid.NewGuid(), "Dima58", "dimochka158@gmail.com", "990Tyeq12-9",null);
            Assert.DoesNotThrow(() => _service.Authenticate(existedUser.Username, existedUser.Password));
            Assert.IsTrue(_service.Authenticate(existedUser.Username, existedUser.Password));
        }

        public void TestLoginNotExistUser()
        {
            Assert.IsFalse(_service.Authenticate("aaaaa", "bbbbb"));
        }  
    }
}
