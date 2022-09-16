using System;
using System.Windows.Input;

namespace ImageCombiner.Commands
{
    public class ActionCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _action.Invoke();
    }
}
