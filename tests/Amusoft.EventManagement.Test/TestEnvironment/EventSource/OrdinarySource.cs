// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventSource
{
	public class CustomVoidParametersHandlerOrdinarySource : IEventSource<CustomVoidParametersHandler>
	{
		private event CustomVoidParametersHandler InvokerEvent
		{
			add { _invoker += value; }
			remove { _invoker -= value; }
		}

		private CustomVoidParametersHandler _invoker;

		public void Add(CustomVoidParametersHandler handler)
		{
			InvokerEvent += handler;
		}

		public void Remove(CustomVoidParametersHandler handler)
		{
			InvokerEvent -= handler;
		}

		/// <inheritdoc />
		public CustomVoidParametersHandler Invoker => (a, b, c) => _invoker?.Invoke(a, b, c);
	}

	public class AsyncEventHandlerOrdinarySource : IEventSource<AsyncEventHandler>
	{
		private event AsyncEventHandler InvokerEvent
		{
			add { _invoker += value; }
			remove { _invoker -= value; }
		}

		private AsyncEventHandler _invoker;

		public void Add(AsyncEventHandler handler)
		{
			InvokerEvent += handler;
		}

		public void Remove(AsyncEventHandler handler)
		{
			InvokerEvent -= handler;
		}

		/// <inheritdoc />
		public AsyncEventHandler Invoker => (s, a) => _invoker?.Invoke(s, a);
	}

	public class EventHandlerOrdinarySource : IEventSource<EventHandler>
	{
		private event EventHandler InvokerEvent
		{
			add { _invoker += value; }
			remove { _invoker -= value; }
		}

		private EventHandler _invoker;

		public void Add(EventHandler handler)
		{
			InvokerEvent += handler;
		}

		public void Remove(EventHandler handler)
		{
			InvokerEvent -= handler;
		}

		/// <inheritdoc />
		public EventHandler Invoker => (s, a) => _invoker?.Invoke(s, a);
	}
}