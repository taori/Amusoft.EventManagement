// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventSource
{
	public class WeakEventSource<TDelegate> : IEventSource<TDelegate> where TDelegate : Delegate
	{
		private WeakEvent<TDelegate> _onEvent = new WeakEvent<TDelegate>();

		public TDelegate Invoker => _onEvent.Invoke;

		public void Add(TDelegate handler)
		{
			_onEvent.Add(handler);
		}

		public void Remove(TDelegate handler)
		{
			_onEvent.Remove(handler);
		}
	}
}