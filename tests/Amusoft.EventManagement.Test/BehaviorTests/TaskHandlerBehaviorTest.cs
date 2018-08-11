// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.EventManagement.Test.TestEnvironment;
using NUnit.Framework;

namespace Amusoft.EventManagement.Test.BehaviorTests
{
	[TestFixture]
	public class TaskHandlerBehaviorTest
	{
		private class TestSource
		{
			public event AsyncEventHandler Event;

			public Task RaiseEvent(object sender, EventArgs args) => Event?.Invoke(sender, args);
		}

		private class TestReceiver
		{
			public async Task ReceiveEvent(object sender, EventArgs args)
			{
				await Task.Delay(1000);
				Interlocked.Increment(ref CallCount);
			}

			public int CallCount;
		}

		[Test]
		public async Task VerifyAwaitedInvocation()
		{
			var receiver = new TestReceiver();
			var source = new TestSource();
			source.Event += receiver.ReceiveEvent;
			source.Event += receiver.ReceiveEvent;
			var start = DateTime.Now;
			await source.RaiseEvent(this, EventArgs.Empty);
			var elapsed = DateTime.Now - start;
			Assert.That(elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1000)));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(1150)));
			Assert.That(receiver.CallCount, Is.EqualTo(2));
		}

		[Test]
		public Task VerifyNonAwaitedInvocation()
		{
			var receiver = new TestReceiver();
			var source = new TestSource();
			source.Event += (receiver.ReceiveEvent);
			source.Event += (receiver.ReceiveEvent);
			var start = DateTime.Now;
			source.RaiseEvent(this, EventArgs.Empty);
			var elapsed = DateTime.Now - start;
			Assert.That(receiver.CallCount, Is.EqualTo(0));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(100)));

			return Task.CompletedTask;
		}
	}
}