﻿// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System.Threading;
using System.Threading.Tasks;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventReceiver
{
	public class CustomVoidParametersHandlerReceiver : EventReceiver<CustomVoidParametersHandler>
	{
		/// <inheritdoc />
		protected override CustomVoidParametersHandler InvocationDelegate()
		{
			return async (a, b, c) => await NotifyExecutionAsync(a, b, c);
		}

		/// <inheritdoc />
		protected override Task OnBeforeNotifyExecution()
		{
			Thread.Sleep(ExecutionTimeSpan);
			return System.Threading.Tasks.Task.CompletedTask;
		}
	}
}