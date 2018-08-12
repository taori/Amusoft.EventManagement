// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using System.Threading.Tasks;
using Amusoft.EventManagement.Test.TestEnvironment;
using Amusoft.EventManagement.Test.TestEnvironment.EventReceiver;
using Amusoft.EventManagement.Test.TestEnvironment.EventSource;
using NUnit.Framework;

namespace Amusoft.EventManagement.Test.IntegrityTests
{
	[TestFixture]
	public class CustomVoidParametersHandlerWeakEventTests : IntegrityTestBase<CustomVoidParametersHandler, CustomVoidParametersHandlerReceiver, WeakEventSource<CustomVoidParametersHandler>>
	{
		[Test]
		public override Task ExpectedExecutionTimeTwoSubOneSource()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			receiver.ExecutionTimeSpan = TimeSpan.FromSeconds(1);
			source.Add(receiver.Receiver());
			source.Add(receiver.Receiver());
			var start = DateTime.Now;
			source.Invoker("1", 2, 3);
			var elapsed = DateTime.Now - start;
			Assert.That(elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1950)));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(2050)));

			return Task.CompletedTask;
		}

		[Test]
		public override Task ExpectedExecutionTimeTwoReceiverOneSource()
		{
			var receiver1 = CreateEventReceiver();
			receiver1.ExecutionTimeSpan = TimeSpan.FromSeconds(1);
			var receiver2 = CreateEventReceiver();
			receiver2.ExecutionTimeSpan = TimeSpan.FromSeconds(1);
			var source = CreateEventSource();
			source.Add(receiver1.Receiver());
			source.Add(receiver2.Receiver());
			var start = DateTime.Now;
			source.Invoker("1", 2, 3);
			var elapsed = DateTime.Now - start;
			Assert.That(elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(1950)));
			Assert.That(elapsed, Is.LessThanOrEqualTo(TimeSpan.FromMilliseconds(2050)));

			return Task.CompletedTask;
		}

		[Test]
		public override Task SubRaiseUnsubRaiseSubRaise()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver.Receiver());
			source.Invoker("1", 2, 3);
			Assert.That(receiver.CallCount, Is.EqualTo(1));
			source.Remove(receiver.Receiver());
			source.Invoker("1", 2, 3);
			Assert.That(receiver.CallCount, Is.EqualTo(1));
			source.Add(receiver.Receiver());
			source.Invoker("1", 2, 3);
			Assert.That(receiver.CallCount, Is.EqualTo(2));

			return Task.CompletedTask;
		}

		[Test]
		public override Task RaiseOnceDoubleSubscribe()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver.Receiver());
			source.Add(receiver.Receiver());
			source.Invoker("1", 2, 3);
			Assert.That(receiver.CallCount, Is.EqualTo(2));

			return Task.CompletedTask;
		}

		[Test]
		public override Task RaiseOnceOneSubscribe()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver.Receiver());
			source.Invoker("1", 2, 3);
			Assert.That(receiver.CallCount, Is.EqualTo(1));

			return Task.CompletedTask;
		}

		[Test]
		public override Task RaiseWithNoSubscribe()
		{
			var receiver = CreateEventReceiver();
			var source = CreateEventSource();
			source.Invoker("1", 2, 3);
			Assert.That(receiver.CallCount, Is.EqualTo(0));

			return Task.CompletedTask;
		}

		[Test]
		public override Task RaiseOnceTwoSubscribers()
		{
			var receiver1 = CreateEventReceiver();
			var receiver2 = CreateEventReceiver();
			var source = CreateEventSource();
			source.Add(receiver1.Receiver());
			source.Add(receiver2.Receiver());
			source.Invoker("1", 2, 3);
			Assert.That(receiver1.CallCount, Is.EqualTo(1));
			Assert.That(receiver2.CallCount, Is.EqualTo(1));

			return Task.CompletedTask;
		}
	}
}