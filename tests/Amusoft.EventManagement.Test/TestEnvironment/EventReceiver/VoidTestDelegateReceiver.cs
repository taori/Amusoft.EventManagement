﻿// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

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
			return Task.CompletedTask;
		}
	}
}