using System.Reflection.Metadata;

namespace Globamantics.Domain
{
    //using record give us value-base equality, a primary constructor, and immutability out of the box which is very suitable for a domain object.
    public abstract record ToDo(Guid Id,
        string Title,
        DateTimeOffset CreatedDate,
        User CreatedBy,
        bool IsCompleted = false,
        bool IsDeleted = false)
    {
        public ToDo? Parent { get; init; } // only going to be allowed to be set when instantiating this record
    }
}