using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Models
{
    public class ToDoTask : ToDo
    {
        public DateTimeOffset DueDate { get; init; }
    }
}
