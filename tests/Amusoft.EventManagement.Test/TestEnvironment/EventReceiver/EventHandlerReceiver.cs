// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventReceiver
{
	public class EventHandlerReceiver : EventReceiver<EventHandler>
	{
		/// <inheritdoc />
		protected override EventHandler InvocationDelegate()
		{
			return async (sender, args) => await this.NotifyExecutionAsync(sender, args);
		}

		/// <inheritdoc />
		protected override Task OnBeforeNotifyExecution()
		{
			Thread.Sleep(ExecutionTimeSpan);
			return System.Threading.Tasks.Task.CompletedTask;
		}
	}
}