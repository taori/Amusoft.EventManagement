// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;
using System.Threading.Tasks;
using Amusoft.EventManagement.Test.TestEnvironment;
using Amusoft.EventManagement.Test.TestEnvironment.EventReceiver;
using Amusoft.EventManagement.Test.TestEnvironment.EventSource;
using NUnit.Framework;

namespace Amusoft.EventManagement.Test.IntegrityTests
{
	[TestFixture]
	public class AsyncEventHandlerWeakEventTests : IntegrityTestBase<AsyncEventHandler, AsyncEventHandlerReceiver, WeakEventSource<AsyncEventHandler>>
	{
		[Test]
		public override async Task ExpectedExecutionTimeTwoSubOneSource()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			receiver.ExecutionTimeSpan = TimeSpan.FromSeconds(1);
			source.Add(receiver.Receiver());
			source.Add(receiver.Receiver());
			var start = DateTime.Now;
			await source.Invoker(this, EventArgs.Empty);
			var elapsed = DateTime.Now - start;
			Assert.That(elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1000)));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(1050)));
		}

		[Test]
		public override async Task ExpectedExecutionTimeTwoReceiverOneSource()
		{
			var receiver1 = CreateEventReceiver();
			receiver1.ExecutionTimeSpan = TimeSpan.FromSeconds(1);
			var receiver2 = CreateEventReceiver();
			receiver2.ExecutionTimeSpan = TimeSpan.FromSeconds(1);
			var source = CreateEventSource();
			source.Add(receiver1.Receiver());
			source.Add(receiver2.Receiver());
			var start = DateTime.Now;
			await source.Invoker(this, EventArgs.Empty);
			var elapsed = DateTime.Now - start;
			Assert.That(elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1000)));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(1050)));
		}

		[Test]
		public override async Task SubRaiseUnsubRaiseSubRaise()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver.Receiver());
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver.CallCount, Is.EqualTo(1));
			source.Remove(receiver.Receiver());
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver.CallCount, Is.EqualTo(1));
			source.Add(receiver.Receiver());
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver.CallCount, Is.EqualTo(2));
		}

		[Test]
		public override async Task RaiseOnceDoubleSubscribe()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver.Receiver());
			source.Add(receiver.Receiver());
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver.CallCount, Is.EqualTo(2));
		}

		[Test]
		public override async Task RaiseOnceOneSubscribe()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver.Receiver());
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver.CallCount, Is.EqualTo(1));
		}

		[Test]
		public override async Task RaiseWithNoSubscribe()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver.CallCount, Is.EqualTo(0));
		}

		[Test]
		public override async Task RaiseOnceTwoSubscribers()
		{
			var receiver1 = CreateEventReceiver();
			var receiver2 = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver1.Receiver());
			source.Add(receiver2.Receiver());
			await source.Invoker(this, EventArgs.Empty);
			Assert.That(receiver1.CallCount, Is.EqualTo(1));
			Assert.That(receiver2.CallCount, Is.EqualTo(1));
		}
	}
}