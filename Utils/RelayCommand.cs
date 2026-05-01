using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantOrderingApp.Utils
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _predicate;
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public RelayCommand(Action<object> action, Func<object, bool> predicate)
        {
            _action = action;
            _predicate = predicate;
        }

        public bool CanExecute(object? parameter)
        {
            if (_predicate == null) return true;

            return _predicate(parameter);
        }

        public void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
