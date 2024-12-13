using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain.Model;
using Todos.Repository.IRepository;
using TodosService.IServices;
using Utilitities;


namespace TodosService.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApiModel _apiModel;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TodoService> _logger;
        private readonly ResponseHelper _responseHelper;
        private readonly ApiResponse _apiResponse; 
       
        public TodoService(ITodoRepository todoRepository, HttpClient httpClient, IOptions<ApiModel> apiModel,
            ICategoryRepository categoryRepository,ILogger<TodoService> logger, ResponseHelper responseHelper, ApiResponse apiResponse)
        {
            _todoRepository = todoRepository;
            _httpClient = httpClient;
            _apiModel = apiModel.Value;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _responseHelper = responseHelper;
            _apiResponse = apiResponse;
        }

        public async Task<TodosResponseMapper> GetAllTodoAppsDataAsync(string location, DateTime? dueDate, string title)
        {
            try
            {
                // Fetch todos from the external API
                var response = await _httpClient.GetAsync(_apiModel.TodosApi);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch todos. Status Code: {response.StatusCode}");
                }

                var todosResponse = await response.Content.ReadAsStringAsync();
                var todos = JsonConvert.DeserializeObject<TodosResponseMapper>(todosResponse);

                if (todos?.Todos == null || !todos.Todos.Any())
                {
                    throw new InvalidOperationException("No todos found in the API response.");
                }

                // Get category by title
                var category = await _categoryRepository.GetCategoryByTitle(title);

                if (category == null)
                {
                    throw new InvalidOperationException($"Category with title '{title}' not found.");
                }

                // Prepare todos for batch insertion
                var todoItems = todos.Todos.Select(todo => new TodoItems
                {
                    TodoId = todo.Id,
                    Todo = todo.Todo,
                    Completed = todo.Completed,
                    UserId = todo.UserId,
                    Location = location,
                    DueDate = dueDate,
                    CategoryId = category.Id // Use existing category ID
                }).ToList();

                // Save todos in bulk
                await _todoRepository.TodosBatchInsertionAsync(todoItems);

                return todos;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching todos from API: {ex.Message}");
                throw; // Re-throw exception to signal failure
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message); // Use Warning for expected failures
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw; // Re-throw exception for unexpected issues
            }
        }

        private WeatherResposeMapper CreateWeatherResponse(string temperature, string condition)
        {
            return new WeatherResposeMapper
            {
                condition = condition ?? "Unknown",
                temperature = temperature ?? "0"
            };
        }


        public async Task<WeatherResposeMapper> GetWeatherForTaskAsync(int todoItemId)
        {
            // getting the Todo item by Id
            var todoItem = await _todoRepository.GetTodoByIdAsync(todoItemId);

            if (todoItem == null || string.IsNullOrEmpty(todoItem.Location))
            {
                return CreateWeatherResponse("0", "Unassigned location");
            }

            try
            {
                var requestUrl = $"{_apiModel.WeatherApi}?key={_apiModel.WeatherApiKey}&q={todoItem.Location}";

                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return CreateWeatherResponse("0", "Error getting weather data. Please try again later");
                }
               
                var weatherResponse = await response.Content.ReadAsStringAsync();

                var deserializedResponse = JsonConvert.DeserializeObject<WeatherResponse>(weatherResponse);

                // Validate the deserialized response
                if (deserializedResponse?.current == null || deserializedResponse.current.condition == null)
                {
                    return CreateWeatherResponse("0", "Invalid weather data obtained");
                }

                return CreateWeatherResponse(
                    deserializedResponse.current.temp_c.ToString(),
                    deserializedResponse.current.condition.text
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while getting weather data: {ex.Message}");
                return CreateWeatherResponse("0", "An error occurred while fetching weather data");
            }
        }

     
        public async Task<StandardResponse> PostTodoAsync(TodosModel todoItems)
        {
            try
            {
                var todo = new TodoItems
                {
                    Todo = todoItems.Todo,
                    UserId = todoItems.UserId,
                    Completed = todoItems.Completed,
                    CategoryId = todoItems.CategoryId,
                    Location = todoItems.Location,
                    Priority = todoItems.Priority,
                    DueDate = todoItems.DueDate
                };
                var response = await _todoRepository.AddTodoAsync(todo);
                return response < 0 ? _apiResponse.createResponse(ResponseStatus.Failed.ToString(), "Todo item already exists or could not be added.", null) :
                    _apiResponse.createResponse(ResponseStatus.Successful.ToString(), $"Todo item added successfully", response.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while processing TodoItems: {ex.Message}");
                return _apiResponse.createResponse(ResponseStatus.Error.ToString(), "An unexpected error occurred while processing request.", null);
            }
        }

        public async Task<StandardResponse> UpdateTodoAsync(TodosUpdateRapper todoItems)
        {
            try
            {
                var todo = new TodoItems
                {
                    Id = todoItems.Id,
                    Todo = todoItems.Todo,
                    UserId = todoItems.UserId,
                    Location = todoItems.Location,
                    DueDate = todoItems.DueDate,
                    CategoryId = todoItems.CategoryId,
                    Completed = todoItems.Completed,
                    Priority = todoItems.Priority
                };

                var response = await _todoRepository.UpdateTodoAsync(todo);

                return response == false ? _apiResponse.createResponse(ResponseStatus.Failed.ToString(), "Todo item failed to update.", null) :
                   _apiResponse.createResponse(ResponseStatus.Successful.ToString(), $"Todo item updated successfully.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while updating TodoItems: {ex.Message}");
                return _apiResponse.createResponse(ResponseStatus.Error.ToString(), "An unexpected error occurred while processing request.", null);
            }
        }


        public async Task<StandardResponse> DeleteTodoAsync(int id)
        {
            try
            {
                var response = await _todoRepository.DeleteTodoByIdAsync(id);

                return response == false ? _apiResponse.createResponse(ResponseStatus.Failed.ToString(), $"Todo item with ID {id} could not be found or deleted.", null) :
                   _apiResponse.createResponse(ResponseStatus.Successful.ToString(), $"Todo item successfully deleted.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while deleting TodoItems: {ex.Message}");
                return _apiResponse.createResponse(ResponseStatus.Error.ToString(), "An unexpected error occurred while processing request.", null);
            }
        }

        public async Task<object> SearchTodosAsync(DataSearch entity)
        {
            try
            {
                var response = await _todoRepository.SearchTodosAsync(entity);
                var (listOfTodos, standardResponse) = _responseHelper.CreateListResponse(response.ToList(), response.Any() ? ResponseStatus.Successful.ToString() :
                    "No record found", null, null);

                // Format the response object to match the required output structure
                return new
                {
                    status = standardResponse.status,
                    todos = listOfTodos
                };
            }
            catch (Exception ex)
            { 
                _logger.LogError($"Error occurred while searching todo item: {ex.Message}");

                return new
                {
                    status = "error",
                    message = "An error occurred while processing your request. Please try again later."
                };
            }
        }

    }
}
