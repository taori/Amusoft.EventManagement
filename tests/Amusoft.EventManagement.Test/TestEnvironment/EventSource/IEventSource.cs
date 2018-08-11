// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;

namespace Amusoft.EventManagement.Test.TestEnvironment.EventSource
{
	public interface IEventSource<TDelegate> where TDelegate : Delegate
	{
		void Add(TDelegate handler);
		void Remove(TDelegate handler);
		TDelegate Invoker { get; } 
	}
}