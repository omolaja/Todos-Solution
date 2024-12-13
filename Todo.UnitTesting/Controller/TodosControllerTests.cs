using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Todos.Api.Controllers;
using TodosService.IServices;
using Todos.Domain.Model;
using TodosService.Services;


public class TodoControllerTests
{
    private readonly Mock<ILogger<TodoService>> _mockLogger;
    private readonly Mock<ITodoService> _mockTodoService;
    private readonly TodosController _controller;

    public TodoControllerTests()
    {
        // Mock the dependencies
        _mockLogger = new Mock<ILogger<TodoService>>();
        _mockTodoService = new Mock<ITodoService>();

        // Pass mocked dependencies to the controller
        _controller = new TodosController(_mockTodoService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task PostTodosItem_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var testTodo = new TodosModel
        {
            Todo = "Test Todo",
            UserId = 1,
            Completed = true,
            CategoryId = 1,
            Location = "Latitude",
            DueDate = DateTime.UtcNow,
            Priority = 3
        };

        var expectedResponse = new StandardResponse
        {
            status = "Successful",
            message = "Todo item added",
            uniqueId = "1"
        };

        _mockTodoService.Setup(s => s.PostTodoAsync(testTodo)).ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.PostTodosItem(testTodo);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var response = Assert.IsType<StandardResponse>(okResult.Value);
        Assert.Equal("Successful", response.status);
        Assert.Equal("Todo item added", response.message);
    }

    [Fact]
    public async Task PostTodosItem_ReturnsBadRequest_WhenNullBody()
    {
        // Act
        var result = await _controller.PostTodosItem(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Request body cannot be null", badRequestResult.Value);
    }

    [Fact]
    public async Task PostTodosItem_ReturnsServerError_OnException()
    {
        // Arrange
        var testTodo = new TodosModel
        {
            Todo = "Test Todo",
            UserId = 1,
            Completed = false,
            CategoryId = 1,
            Location = "Test Location",
            DueDate = DateTime.UtcNow,
            Priority = 3
        };

        _mockTodoService.Setup(s => s.PostTodoAsync(testTodo)).ThrowsAsync(new System.Exception("Database error"));

        // Act
        var result = await _controller.PostTodosItem(testTodo);

        // Assert
        var serverErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, serverErrorResult.StatusCode);
        Assert.Equal("oops something went wrong. Please try again", serverErrorResult.Value);
    }
}
