using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.Api.Controllers;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Application.Services;
using Xunit;

namespace TodoApp.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoRepository> _mockTodoRepository;
        private readonly TodoService _todoService;
        private readonly TodoController _todoController;

        public TodoControllerTests()
        {
            // Initialize the mock repository
            _mockTodoRepository = new Mock<ITodoRepository>();

            // Initialize the service using the mock repository
            _todoService = new TodoService(_mockTodoRepository.Object);

            // Initialize the controller using the service
            _todoController = new TodoController(_todoService);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTodos()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test 1", IsCompleted = false },
                new TodoItem { Id = 2, Title = "Test 2", IsCompleted = true }
            };
            _mockTodoRepository.Setup(repo => repo.GetAllItem()).ReturnsAsync(todoItems);

            // Act
            var result = await _todoController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TodoItem>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Test 1", IsCompleted = false };
            _mockTodoRepository.Setup(repo => repo.GetItemById(1)).ReturnsAsync(todoItem);

            // Act
            var result = await _todoController.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal("Test 1", returnValue.Title);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTodoItemNotFound()
        {
            // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _mockTodoRepository.Setup(repo => repo.GetItemById(1)).ReturnsAsync((TodoItem)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // Act
            var result = await _todoController.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "New Todo", IsCompleted = false };
            _mockTodoRepository.Setup(repo => repo.AddItem(todoItem)).Returns(Task.CompletedTask);

            // Act
            var result = await _todoController.Create(todoItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal("New Todo", returnValue.Title);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenTodoItemUpdated()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Updated Title", IsCompleted = true };
            _mockTodoRepository.Setup(repo => repo.UpdateItem(todoItem)).Returns(Task.CompletedTask);

            // Act
            var result = await _todoController.Update(1, todoItem);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Updated Title", IsCompleted = true };

            // Act
            var result = await _todoController.Update(2, todoItem);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTodoItemDeleted()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Test 1", IsCompleted = false };
            _mockTodoRepository.Setup(repo => repo.GetItemById(1)).ReturnsAsync(todoItem);
            _mockTodoRepository.Setup(repo => repo.DeleteItem(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _todoController.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTodoItemNotFound()
        {
            // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _mockTodoRepository.Setup(repo => repo.GetItemById(1)).ReturnsAsync((TodoItem)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // Act
            var result = await _todoController.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Reset_ReturnsNoContent_WhenTodosDeletedAndReset()
        {
            // Arrange
            _mockTodoRepository.Setup(repo => repo.DeleteAndReset()).Returns(Task.CompletedTask);

            // Act
            var result = await _todoController.Reset();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
