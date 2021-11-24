using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BESM3CAData.Model
{
    public class RelayCommand : ICommand
    {
        private Action commandTask;
        private Func<bool> canExecute;

        public RelayCommand(Action workToDo, Func<bool> workCanBeDone)
        {
            commandTask = workToDo;
            canExecute = workCanBeDone;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            commandTask();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
