using CommunityToolkit.Mvvm.Messaging.Messages;
using Globamantics.Domain;

namespace Globomantics.Windows.Messages
{
    public class ToDoDeletedMessage : ValueChangedMessage<ToDo>
    {
        public ToDoDeletedMessage(ToDo value) : base(value)
        {
        }
    }
}
