using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.DataContext.Persistence;
using Todos.Domain.Model;
using Todos.Repository.IRepository;

namespace Todos.Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TodoContext _todoContext;

        public CategoryRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task<Category?> GetCategoryByTitle(string title)
        {
            return await _todoContext.Categories.FirstOrDefaultAsync(c => c.Title == title);
        }
    }
}
