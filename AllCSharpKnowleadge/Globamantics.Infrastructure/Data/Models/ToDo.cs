using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Models
{
    public abstract class ToDo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = default!;
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedById { get; set; } = default!;

        public virtual ToDo? Parent { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
    }
}
