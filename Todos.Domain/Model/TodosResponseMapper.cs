using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Domain.Model
{
    public class TodosMapper : DataObject
    {
        public string? Todo { get; set; }
        public bool Completed { get; set; } = false;
        public int UserId { get; set; }
    }

    public class TodosResponseMapper
    {
        public List<TodosMapper?> Todos { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
