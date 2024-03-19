using CommunityToolkit.Mvvm.Messaging.Messages;
using Globamantics.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Windows.Messages
{
    public class ToDoSavedMessage : ValueChangedMessage<ToDo>
    {
        public ToDoSavedMessage(ToDo value) : base(value)
        {
        }
    }
}
