using Globomantics.Windows.UserControls;
using Globomantics.Windows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Globomantics.Windows.Factory
{
    public class ToDoUserControlFactory
    {
        public static UserControl CreateUserControl(IToDoViewModel viewModel)
        {
            UserControl control = viewModel switch
            {
                BugViewModel => new BugControl(viewModel),
                FeatureViewModel => new FeatureControl(viewModel),
                _ => throw new NotImplementedException()
            };

            return control;
        }
    }
}
