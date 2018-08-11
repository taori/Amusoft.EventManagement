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
using System.Windows.Shapes;

namespace Amusoft.EventManagement.WindowTests
{
	/// <summary>
	/// Interaktionslogik für ChildWindow.xaml
	/// </summary>
	public partial class ChildWindow : Window
	{
		private readonly MainWindow _mainWindow;
		private readonly byte[] _allocation;

		public ChildWindow(MainWindow mainWindow)
		{
			_mainWindow = mainWindow;
			InitializeComponent();

			_allocation = new byte[50000000];

			switch (_mainWindow.EventType)
			{
				case EventTypes.Ordinary:
					_mainWindow.EventOrdinary += MainWindowOnEventOrdinary;
					break;
				case EventTypes.WeakEvent:
					_mainWindow.WeakEvent += MainWindowOnEventOrdinary;
					break;
				case EventTypes.WeakEventSource:
					_mainWindow.WeakEventSource += MainWindowOnEventOrdinary;
					break;
				case EventTypes.AsyncWeakEvent:
					_mainWindow.AsyncWeakEvent += MainWindowOnAsyncWeakEvent;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private async Task MainWindowOnAsyncWeakEvent(object target, string s)
		{
			await Task.Delay(1000);
			TextBlock.Text = s;
		}

		private async void MainWindowOnEventOrdinary(object sender, string s)
		{
			await Task.Delay(1000);
			TextBlock.Text = s;
		}

		protected override void OnClosed(EventArgs e)
		{
			if (_mainWindow.IsUnloadedOnClose)
			{
				switch (_mainWindow.EventType)
				{
					case EventTypes.Ordinary:
						_mainWindow.EventOrdinary -= MainWindowOnEventOrdinary;
						break;
					case EventTypes.WeakEvent:
						_mainWindow.WeakEvent -= MainWindowOnEventOrdinary;
						break;
					case EventTypes.WeakEventSource:
						_mainWindow.WeakEventSource -= MainWindowOnEventOrdinary;
						break;
					case EventTypes.AsyncWeakEvent:
						_mainWindow.AsyncWeakEvent -= MainWindowOnAsyncWeakEvent;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			base.OnClosed(e);
		}
	}
}
