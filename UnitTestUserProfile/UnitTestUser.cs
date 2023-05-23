using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserProfileAPI.Controllers;
using UserProfileAPI.Models;
using UserProfileAPI.Services;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Tests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController controller;
        private Mock<IUsersRepository> mockUsersRepository;

        [SetUp]
        public void Setup()
        {
            mockUsersRepository = new Mock<IUsersRepository>();
            controller = new UsersController(mockUsersRepository.Object);
        }

        [Test]
        public async Task Get_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User>()
            {
                new User { UserId = "user1", UserName = "John Doe" },
                new User { UserId = "user2", UserName = "Jane Smith" }
            };
            mockUsersRepository.Setup(r => r.GetUsers()).ReturnsAsync(users);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(users, okResult.Value);
        }

        [Test]
        public async Task GetById_ExistingUserId_ReturnsUser()
        {
            // Arrange
            var userId = "user1";
            var user = new User { UserId = userId, UserName = "John Doe" };
            mockUsersRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await controller.GetById(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(user, okResult.Value);
        }

        [Test]
        public async Task GetById_NonExistingUserId_ReturnsNotFound()
        {
            // Arrange
            var userId = "nonexistinguser";
            mockUsersRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync((User)null);

            // Act
            var result = await controller.GetById(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
