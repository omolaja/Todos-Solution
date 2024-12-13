using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Domain.Model
{
    public class Category : DataObject
    {
        public string? Title { get; set; }

        [MaxLength(100)]
        public string? category { get; set; } // Optional
    }
}
