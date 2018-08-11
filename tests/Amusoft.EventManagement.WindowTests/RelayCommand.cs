using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Amusoft.EventManagement.WindowTests
{
	public class RelayCommand<T> : ICommand
	{

		#region Declarations

		readonly Predicate<T> _canExecute;
		readonly Action<T> _execute;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		public RelayCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{

			if (execute == null)
				throw new ArgumentNullException("execute");
			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion

		#region ICommand Members

		public event EventHandler CanExecuteChanged;

		public virtual bool CanExecute(Object parameter)
		{
			return _canExecute == null ? true : _canExecute((T)parameter);
		}

		public virtual void Execute(Object parameter)
		{
			_execute((T)parameter);
		}

		#endregion
	}
}
