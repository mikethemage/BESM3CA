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
        private readonly Action _commandTask;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action workToDo, Func<bool> workCanBeDone)
        {
            _commandTask = workToDo;
            _canExecute = workCanBeDone;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        public void Execute(object? parameter)
        {
            _commandTask();
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
