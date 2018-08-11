// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

namespace Amusoft.EventManagement.Test.TestEnvironment.EventReceiver
{
	public class AsyncEventHandlerReceiver : EventReceiver<AsyncEventHandler>
	{
		/// <inheritdoc />
		protected override AsyncEventHandler InvocationDelegate()
		{
			return async (sender, args) => await this.NotifyExecutionAsync(sender, args);
		}
	}
}