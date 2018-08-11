// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details
using System;
using System.Collections.Generic;
using System.Threading;
using Amusoft.EventManagement.Compatibility;

namespace Amusoft.EventManagement
{
	public class WeakEvent<THandler> where THandler : Delegate
	{
		private THandler _invoker;

		public THandler Invoke
		{
			get
			{
				lock (_lock)
				{
					if (_invoker == null)
						return _invoker = WeakEventDelegateFactory.Create(default(THandler), _handles);
					return _invoker;
				}
			}
		}

		private readonly List<WeakDelegate<THandler>> _handles = new List<WeakDelegate<THandler>>();

		private readonly object _lock = new object();

		public void Add(THandler @delegate)
		{
			bool lockTaken = false;
			try
			{
				lockTaken = Monitor.TryEnter(_lock);
				if (lockTaken && _invoker == null)
					_invoker = WeakEventDelegateFactory.Create(@delegate, _handles);
				_handles.Insert(0, new WeakDelegate<THandler>(CompatReflectionExtensions.GetMethodInfo(@delegate), @delegate.Target));
			}
			finally
			{
				if (lockTaken)
					Monitor.Exit(_lock);
			}
		}

		public void Remove(THandler @delegate)
		{
			var index = _handles.FindIndex(del => del.IsMatch(@delegate));
			if (index >= 0)
				_handles.RemoveAt(index);
		}
	}
}