using Todos.Domain.Model;

namespace Todos.Repository.IRepository
{
    public interface ITodoRepository
    {
        Task<int> AddTodoAsync(TodoItems todo);
        Task TodosBatchInsertionAsync(IEnumerable<TodoItems> todos);
        Task<bool> DeleteTodoByIdAsync(int id);
        Task<IEnumerable<TodoItems>> GetAllTodosAsync();
        Task<TodoItems> GetTodoByIdAsync(int id);
        Task<IEnumerable<TodoItems>> SearchTodosAsync(DataSearch entity);
        Task<bool> UpdateTodoAsync(TodoItems todo);
    }
}