// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using System.Threading.Tasks;
using Amusoft.EventManagement.Test.TestEnvironment.EventReceiver;
using Amusoft.EventManagement.Test.TestEnvironment.EventSource;

namespace Amusoft.EventManagement.Test.TestEnvironment
{
	public abstract class EventTestBase<TDelegate, TReceiver, TEventSource>
		where TDelegate : Delegate
		where TReceiver : IEventReceiver<TDelegate>, new()
		where TEventSource : IEventSource<TDelegate>, new()
	{
		protected virtual TReceiver CreateEventReceiver() => new TReceiver();
		protected virtual TEventSource CreateEventSource() => new TEventSource();
	}

	public abstract class IntegrityTestBase<TDelegate, TReceiver, TEventSource>
		: EventTestBase<TDelegate, TReceiver, TEventSource>
		where TDelegate : Delegate
		where TReceiver : IEventReceiver<TDelegate>, new()
		where TEventSource : IEventSource<TDelegate>, new()
	{
		public abstract Task RaiseOnceDoubleSubscribe();
		public abstract Task RaiseOnceOneSubscribe();
		public abstract Task RaiseWithNoSubscribe();
		public abstract Task RaiseOnceTwoSubscribers();
		public abstract Task SubRaiseUnsubRaiseSubRaise();
		public abstract Task ExpectedExecutionTimeTwoSubOneSource();
		public abstract Task ExpectedExecutionTimeTwoReceiverOneSource();
	}
}