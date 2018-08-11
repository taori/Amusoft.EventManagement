using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeakEvent;

namespace Amusoft.EventManagement.WindowTests
{
	public enum EventTypes
	{
		Ordinary,
		WeakEvent,
		WeakEventSource,
		AsyncWeakEvent
	}

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			CollectGarbageCommand = new RelayCommand<object>(o =>
			{
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
			});

			RaiseEventCommand = new RelayCommand<object>(o =>
			{
				var handler = EventOrdinary;
				if (handler != null)
					handler(this, DateTime.Now.ToString());

				_weakEvent.Invoke(this, DateTime.Now.ToString());
				_weakEventSource.Raise(this, DateTime.Now.ToString());
				_asyncWeakEvent.Invoke(this, DateTime.Now.ToString());
			});

			OpenChildWindowCommand = new RelayCommand<object>(o =>
			{
				var w = new ChildWindow(this);
				w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				w.Show();
			});
		}

		public static readonly DependencyProperty EventTypeProperty = DependencyProperty.Register(
			nameof(EventType), typeof (EventTypes), typeof (MainWindow), new PropertyMetadata(EventTypes.AsyncWeakEvent));

		public EventTypes EventType
		{
			get { return (EventTypes) GetValue(EventTypeProperty); }
			set { SetValue(EventTypeProperty, value); }
		}

		public static readonly DependencyProperty IsUnloadedOnCloseProperty = DependencyProperty.Register(
			nameof(IsUnloadedOnClose), typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

		public bool IsUnloadedOnClose
		{
			get { return (bool)GetValue(IsUnloadedOnCloseProperty); }
			set { SetValue(IsUnloadedOnCloseProperty, value); }
		}

		public static readonly DependencyProperty CollectGarbageCommandProperty = DependencyProperty.Register(
			nameof(CollectGarbageCommand), typeof(ICommand), typeof(MainWindow), new PropertyMetadata(default(ICommand)));

		public ICommand CollectGarbageCommand
		{
			get { return (ICommand)GetValue(CollectGarbageCommandProperty); }
			set { SetValue(CollectGarbageCommandProperty, value); }
		}

		public static readonly DependencyProperty OpenChildWindowCommandProperty = DependencyProperty.Register(
			nameof(OpenChildWindowCommand), typeof(ICommand), typeof(MainWindow), new PropertyMetadata(default(ICommand)));

		public ICommand OpenChildWindowCommand
		{
			get { return (ICommand)GetValue(OpenChildWindowCommandProperty); }
			set { SetValue(OpenChildWindowCommandProperty, value); }
		}

		public static readonly DependencyProperty RaiseEventCommandProperty = DependencyProperty.Register(
			nameof(RaiseEventCommand), typeof (ICommand), typeof (MainWindow), new PropertyMetadata(default(ICommand)));

		public ICommand RaiseEventCommand
		{
			get { return (ICommand) GetValue(RaiseEventCommandProperty); }
			set { SetValue(RaiseEventCommandProperty, value); }
		}

		public event EventHandler<string> EventOrdinary;

		private WeakEventSource<string> _weakEventSource = new WeakEventSource<string>();
		public event EventHandler<string> WeakEventSource
		{
			add { _weakEventSource.Subscribe(value); }
			remove { _weakEventSource.Unsubscribe(value); }
		}

		public delegate Task AsyncEventHandler<TArgs>(object sender, TArgs args);

		private WeakEvent<EventHandler<string>> _weakEvent = new WeakEvent<EventHandler<string>>();
		public event EventHandler<string> WeakEvent
		{
			add { _weakEvent.Add(value); }
			remove { _weakEvent.Remove(value); }
		}

		private readonly WeakEvent<AsyncEventHandler<string>> _asyncWeakEvent = new WeakEvent<AsyncEventHandler<string>>();

		public event AsyncEventHandler<string> AsyncWeakEvent
		{
			add { _asyncWeakEvent.Add(value); }
			remove { _asyncWeakEvent.Remove(value); }
		}
	}
}
