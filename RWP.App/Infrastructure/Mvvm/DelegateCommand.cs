using System;
using System.Windows.Input;

namespace RWP.App.Infrastructure.Mvvm
{
	public class DelegateCommand : ICommand
	{
		private readonly Action _handler;
		private readonly Func<bool> _condition; 

		public DelegateCommand(Action handler, Func<bool> condition = null)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");

			_handler = handler;
			_condition = condition;
		}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}

		public bool CanExecute()
		{
			return _condition != null ? _condition() : true;
		}

		public void Execute(object parameter)
		{
			_handler();
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged(this, EventArgs.Empty);
		}
	}

	public class DelegateCommand<T> : ICommand
	{
		private readonly Action<T> _handler;
		private readonly Func<T, bool> _condition;

		public DelegateCommand(Action<T> handler, Func<T, bool> condition = null)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");

			_handler = handler;
			_condition = condition;
		}

    bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T)parameter);
		}

		public bool CanExecute(T parameter)
		{
			return _condition != null ? _condition(parameter) : true;
		}

		public void Execute(object parameter)
		{
			_handler((T)parameter);
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public void RaiseCanExecuteChanged(T parameter)
		{
			CanExecuteChanged(this, EventArgs.Empty);
		}
	}
}