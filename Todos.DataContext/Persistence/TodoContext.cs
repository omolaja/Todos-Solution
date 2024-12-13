using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain.Model;

namespace Todos.DataContext.Persistence
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public DbSet<TodoItems> Todos { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Beginner", category="1" },
                 new Category { Id = 2, Title = "Intermediate", category = "2" },
                new Category { Id = 3, Title = "Experienced", category = "3" }
                );
        }

    }
}
