using Todos.Domain.Model;

namespace TodosService.IServices
{
    public interface ITodoService
    {
        Task<StandardResponse> DeleteTodoAsync(int id);
        Task<TodosResponseMapper> GetAllTodoAppsDataAsync(string Location, DateTime? DueDate, string title);

        Task<WeatherResposeMapper> GetWeatherForTaskAsync(int todoItemId);

        Task<StandardResponse> PostTodoAsync(TodosModel todoItems);
       
        Task<object> SearchTodosAsync(DataSearch entity);

        Task<StandardResponse> UpdateTodoAsync(TodosUpdateRapper todoItems);
    }
}