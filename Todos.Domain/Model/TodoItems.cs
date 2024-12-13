using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Domain.Model
{
    public class TodoItems : DataObject
    {
       
        public int TodoId { get; set; } = 0;// Id returned from https://dummyjson.com/todos

        [Required]
        [MaxLength(500)]
        public string? Todo { get; set; }
        public bool Completed { get; set; } = false;

        [Required]
        public int UserId { get; set; }

        // Additional Fields
        public int? Priority { get; set; } = 3; // Default to 3

        [MaxLength(100)]
        public string? Location { get; set; } // Optional, can be formatted as "latitude,longitude"
        public DateTime? DueDate { get; set; } // Optional

        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class TodosModel
    {
        [Required]
        [MaxLength(500)]
        public string? Todo { get; set; }

        [Required]
        public bool Completed { get; set; } = false;

        [Required]
        public int UserId { get; set; }

        public int? CategoryId { get; set; }
        public string? Location { get; set; } = "longitude";

        public DateTime? DueDate { get; set; } // Optional

        public int? Priority { get; set; } 
    }

    public class TodosUpdateRapper : TodosModel
    {
        public int Id { get; set; }
    }

    public class StandardResponse
    {
        public string? status { get; set; }
        public string? uniqueId { get; set; }
        public string? message { get; set; }
    }
}
