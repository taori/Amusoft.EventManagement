// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventReceiver
{
	public interface IEventReceiver<TDelegate> where TDelegate : Delegate
	{
		int CallCount { get; }
		Func<TDelegate> Receiver { get; }
		event EventHandler<object[]> Executed;
	}
}