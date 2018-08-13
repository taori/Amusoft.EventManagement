// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventReceiver
{
	public abstract class EventReceiver<TDelegate> : IEventReceiver<TDelegate> where TDelegate : Delegate
	{
		protected abstract TDelegate InvocationDelegate();

		public Func<TDelegate> Receiver => InvocationDelegate;

		public int CallCount { get; private set; }

		public event EventHandler<object[]> Executed;

		public TimeSpan ExecutionTimeSpan { get; set; }

		protected virtual Task OnBeforeNotifyExecution() => Task.Delay(ExecutionTimeSpan);

		protected async Task NotifyExecutionAsync(params object[] eventArguments)
		{
			await OnBeforeNotifyExecution();
			CallCount++;
			Executed?.Invoke(this, eventArguments);
		}
	}
}