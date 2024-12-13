using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.DataContext.Persistence;
using Todos.Domain.Model;
using Todos.Repository.IRepository;

namespace Todos.Repository.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _todoContext;
        private readonly ILogger<TodoRepository> _logger;

        public TodoRepository(TodoContext todoContext, ILogger<TodoRepository> logger)
        {
            _todoContext = todoContext;
            _logger = logger;
        }

        public async Task<IEnumerable<TodoItems>> GetAllTodosAsync()
        {
            return await _todoContext.Todos.Include(c => c.Category).ToListAsync();
        }
        public async Task<TodoItems?> GetTodoByIdAsync(int id)
        {
            return await _todoContext.Todos
                .AsNoTracking()
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
       
        public async Task<int> AddTodoAsync(TodoItems todo)
        {
            try
            {
                var todoCheck = await _todoContext.Todos.FirstOrDefaultAsync(t => t.Todo == todo.Todo
                && t.UserId == todo.UserId
                && t.Completed == todo.Completed);

                if (todoCheck != null)
                {
                    return -1; // Indicates that the item already exists
                }

                // Ensuring CategoryId is null if not provided
                if (todo.CategoryId == null || !await _todoContext.Categories.AnyAsync(c => c.Id == todo.CategoryId))
                {
                    todo.CategoryId = null;
                }

                await _todoContext.Todos.AddAsync(todo);
                await _todoContext.SaveChangesAsync();
                return todo.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task TodosBatchInsertionAsync(IEnumerable<TodoItems> todos)
        {
            if (todos == null || !todos.Any())
            {
                throw new ArgumentException("No todos to add.", nameof(todos));
            }

            // Fetch existing TodoIds
            var existingTodoIds = await _todoContext.Todos
                .AsNoTracking()
                .Where(t => todos.Select(td => td.TodoId).Contains(t.TodoId))
                .Select(t => t.TodoId)
                .ToListAsync();

            // Filter out duplicates
            var newTodos = todos.Where(t => !existingTodoIds.Contains(t.TodoId)).ToList();

            if (!newTodos.Any())
            {
                _logger.LogInformation("No new todo items to add.");
                return;
            }

            // saving batch todo data
            try
            {
                await _todoContext.Todos.AddRangeAsync(newTodos);
                await _todoContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error saving todos: {ex.Message}");
                throw new InvalidOperationException("Failed to save todo items.", ex);
            }
        }

        public async Task<bool> UpdateTodoAsync(TodoItems todo)
        {
            try
            {
                var todoCheck = await GetTodoByIdAsync(todo.Id);
                if (todoCheck == null)
                {
                    return false;
                }

                _todoContext.Todos.Update(todo);
                await _todoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating todos: {ex.Message}");
                return false;
            }

        }
        public async Task<bool> DeleteTodoByIdAsync(int id)
        {
            try
            {
                var todoCheck = await GetTodoByIdAsync(id);
                if (todoCheck == null)
                {
                    return false;
                }

                _todoContext.Todos.Remove(todoCheck);
                await _todoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting todos: {ex.Message}");
                return false;
            }
        }
        public async Task<IEnumerable<TodoItems>> SearchTodosAsync(DataSearch entity)
        {
            var query = _todoContext.Todos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(entity.title))
            {
                query = query.Where(t => t.Category != null && t.Category.Title.Contains(entity.title));
            }

            // Filter by priority if it is greater than 0
            if (entity.priority.HasValue && entity.priority.Value > 0)
            {
                query = query.Where(t => t.Priority == entity.priority.Value);
            }

            // Filter by dueDate if it has a value and is not an empty string
            if (entity.dueDate.HasValue && entity.dueDate.Value != DateTime.MinValue)
            {
                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == entity.dueDate.Value.Date);
            }

            return await query.ToListAsync();
        }

    }
}
