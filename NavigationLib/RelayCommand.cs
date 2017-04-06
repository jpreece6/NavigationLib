using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationLib
{
    public class RelayCommand
    {
        public delegate void CanExecuteChangedHandler(object sender, EventArgs e);
        public event CanExecuteChangedHandler CanExecuteChanged;

        private Action _command;
        private Func<bool> _executionPredicate;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _command = execute;
            _executionPredicate = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            var result = _executionPredicate();

            if (result == false)
                return;

            CanExecuteChanged?.Invoke(this, new EventArgs());
            _command();
        }
    }
}
