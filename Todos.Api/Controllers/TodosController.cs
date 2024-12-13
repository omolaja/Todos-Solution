using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todos.Domain.Model;
using TodosService.IServices;
using TodosService.Services;

namespace Todos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoService> _logger;
        public TodosController (ITodoService todoService, ILogger<TodoService> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet("fetch-todo-data")]
        public async Task<IActionResult> GetFetchTodoData([FromQuery] string location, DateTime? dueDate, string title)
        {
            var todos = await _todoService.GetAllTodoAppsDataAsync(location, dueDate, title);
            return Ok(todos);
        }

        [HttpGet("weather-condition")]
        public async Task<IActionResult> GetWeatherCondition([FromQuery] int id)
        {
            var weatherResponse = await _todoService.GetWeatherForTaskAsync(id);
            return Ok(weatherResponse);
        }

        [HttpPost("post-todos-item")]
        public async Task<IActionResult> PostTodosItem([FromBody] TodosModel todosModel)
        {
            if(todosModel == null)
            {
                _logger.LogWarning("Received null todos item in Todo Creation");
                return BadRequest("Request body cannot be null");
            }

            _logger.LogInformation("Request payload {&todosModel}", todosModel);

            try
            {
                var todoResponse = await _todoService.PostTodoAsync(todosModel);
                if(todoResponse == null)
                {
                    return BadRequest("An error occured while processing your request.");
                }
                return Ok(todoResponse);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception occured while processing your request");
                return StatusCode(500, "oops something went wrong. Please try again");
            }
        }

        [HttpPost("update-todos-item")]
        public async Task<IActionResult> UpdateTodosItem([FromBody] TodosUpdateRapper todoItems)
        {
            if (todoItems == null)
            {
                _logger.LogWarning("Received null todos item in Todo update");
                return BadRequest("Request body cannot be null");
            }

            _logger.LogInformation("Request payload {&todoItems}", todoItems);

            try
            {
                var todoResponse = await _todoService.UpdateTodoAsync(todoItems);
                if (todoResponse == null)
                {
                    return BadRequest("An error occured while processing your request.");
                }
                return Ok(todoResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while processing your request");
                return StatusCode(500, "oops something went wrong. Please try again");
            }
        }

        [HttpPost("delete-todos-item")]
        public async Task<IActionResult> DeleteTodosItem([FromBody] DataObject dataObject)
        {
            try
            {
                if (dataObject.Id <= 0)
                {
                    return BadRequest("Invalid Todo ID.");
                }

                var response = await _todoService.DeleteTodoAsync(dataObject.Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting Todo item with ID {dataObject.Id}.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost("search-todos-item")]
        public async Task<IActionResult> SearchTodosItem([FromBody] DataSearch dataSearch)
        {
            if (dataSearch == null)
            {
                return BadRequest("The search data cannot be null.");
            }

            if (dataSearch.dueDate == null)
            {
                return BadRequest("Invalid Due Date supplied.");
            }

            try
            {
                var response = await _todoService.SearchTodosAsync(dataSearch);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching Todo items.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing your request.");
            }
        }


    }
}
