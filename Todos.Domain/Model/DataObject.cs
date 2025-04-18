﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Domain.Model
{
    public class DataObject
    {
        public int Id { get; set; }
    }

    public class DataSearch
    {
        public string? title { get; set; }
        public int? priority { get; set; }
        public DateTime? dueDate { get; set; }
    }

}
