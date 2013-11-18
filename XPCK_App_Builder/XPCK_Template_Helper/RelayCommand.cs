using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XPCK_Template_Helper
{
    public class RelayCommand : ICommand
    {
        Action Action;
        public RelayCommand(Action action)
        {
            Action = action;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }
        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
        void ICommand.Execute(object parameter)
        {
            Action.Invoke();
        }
    }
}
