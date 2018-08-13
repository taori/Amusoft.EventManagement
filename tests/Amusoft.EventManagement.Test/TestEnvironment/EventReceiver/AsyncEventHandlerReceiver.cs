// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

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