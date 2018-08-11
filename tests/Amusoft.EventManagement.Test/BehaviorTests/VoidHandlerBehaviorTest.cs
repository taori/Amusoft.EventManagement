// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using System.Threading;
using NUnit.Framework;

namespace Amusoft.EventManagement.Test.BehaviorTests
{
	[TestFixture]
	public class VoidHandlerBehaviorTest 
	{
		private class TestSource
		{
			public event EventHandler Event;

			public void RaiseEvent(object sender, EventArgs args) => Event?.Invoke(sender, args);
		}

		private class TestReceiver
		{
			public void ReceiveEvent(object sender, EventArgs args) => Thread.Sleep(1000);
		}

		[Test]
		public void ExpectedExecutionTimeTwoSubOneSource()
		{
			var receiver = new TestReceiver();
			var source = new TestSource();
			source.Event += (receiver.ReceiveEvent);
			source.Event += (receiver.ReceiveEvent);
			var start = DateTime.Now;
			source.RaiseEvent(this, EventArgs.Empty);
			var elapsed = DateTime.Now - start;
			Assert.That(elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1950)));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(2050)));
		}
	}
}