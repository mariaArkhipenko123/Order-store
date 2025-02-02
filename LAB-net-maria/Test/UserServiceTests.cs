using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Auth;
using Lab.Application.Models.DTOs.Secure;
using Lab.Application.Services;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Test
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IAuthUserService> _mockAuthUserService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAuthUserService = new Mock<IAuthUserService>();
            _userService = new UserService(_mockUnitOfWork.Object, _mockAuthUserService.Object);
        }

        [Fact]
        public async Task RegisterUser()
        {
            var authorDTO = new АuthorDTO();  
            var expectedResult = IdentityResult.Success;

            _mockAuthUserService.Setup(x => x.RegisterAsync(authorDTO)).ReturnsAsync(expectedResult);

            var result = await _userService.RegisterAsync(authorDTO);
 
            Assert.Equal(expectedResult, result);
            _mockAuthUserService.Verify(x => x.RegisterAsync(authorDTO), Times.Once);
        }

        [Fact]
        public async Task LoginUser()
        { 
            var authorDTO = new АuthorDTO();  
            var expectedResult = (IdentityResult.Success, "token", "refreshToken");

            _mockAuthUserService.Setup(x => x.LoginAsync(authorDTO)).ReturnsAsync(expectedResult);

            var result = await _userService.LoginAsync(authorDTO);
 
            Assert.Equal(expectedResult, result);
            _mockAuthUserService.Verify(x => x.LoginAsync(authorDTO), Times.Once);
        }

        [Fact]
        public async Task GetAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(),FirstName = "Test User1" },
                new User { Id = Guid.NewGuid(),FirstName = "Test User2" }
            };

            _mockUnitOfWork.Setup(x => x.Users.GetAllAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();
 
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUnitOfWork.Verify(x => x.Users.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUser()
        { 
            var user = new User { Id = Guid.NewGuid(), FirstName = "Test User" }; 
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var authUserServiceMock = new Mock<IAuthUserService>();
            unitOfWorkMock.Setup(x => x.Users.AddAsync(user)).Returns(Task.CompletedTask);

            var userService = new UserService(unitOfWorkMock.Object, authUserServiceMock.Object);

            await userService.AddUserAsync(user);
  
            unitOfWorkMock.Verify(x => x.Users.AddAsync(user), Times.Once);
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUser()
        {
            var user = new User { Id = Guid.NewGuid(), FirstName = "Test User" }; 
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var authUserServiceMock = new Mock<IAuthUserService>();
            unitOfWorkMock.Setup(u => u.Users.UpdateAsync(user)).Returns(Task.CompletedTask);

            var userService = new UserService(unitOfWorkMock.Object, authUserServiceMock.Object);

            await userService.UpdateUserAsync(user);

            unitOfWorkMock.Verify(u => u.Users.UpdateAsync(user), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task DeleteUser()
        {
            var userId = Guid.NewGuid();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var authUserServiceMock = new Mock<IAuthUserService>();
            unitOfWorkMock.Setup(u => u.Users.DeleteAsync(userId)).Returns(Task.CompletedTask);

            var userService = new UserService(unitOfWorkMock.Object, authUserServiceMock.Object);

            await userService.DeleteUserAsync(userId);

            unitOfWorkMock.Verify(u => u.Users.DeleteAsync(userId), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserById()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FirstName = "User3" };

            _mockUnitOfWork.Setup(x => x.Users.GetByIdAsync(userId)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(userId);
  
            Assert.Equal(user, result);
            _mockUnitOfWork.Verify(x => x.Users.GetByIdAsync(userId), Times.Once);
        }
    }
}