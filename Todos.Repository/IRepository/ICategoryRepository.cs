using Todos.Domain.Model;

namespace Todos.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryByTitle(string title);
    }
}