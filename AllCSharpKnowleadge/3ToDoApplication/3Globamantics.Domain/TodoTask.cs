 using System.Xml.Schema;

namespace Globamantics.Domain
{
    public record TodoTask(string Title, DateTimeOffset DueDate, User CreatedBy) : ToDo(Guid.NewGuid(), Title, DateTimeOffset.UtcNow, CreatedBy);
}