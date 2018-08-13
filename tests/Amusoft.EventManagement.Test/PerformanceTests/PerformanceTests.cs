// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;
using System.Diagnostics;
using Amusoft.EventManagement.Test.TestEnvironment;
using NUnit.Framework;

namespace Amusoft.EventManagement.Test.PerformanceTests
{
	[TestFixture]
	public class PerformanceTests
	{
		[Test]
#if DEBUG
		[TestCase(1, 50, 1_000_000)]
		[TestCase(10, 50, 1_000_000)]
#else
		[TestCase(1, 10, 1_000_000)]
		[TestCase(10, 10, 1_000_000)]
#endif
		public void PerformanceTest(int attachCount, int expectedPerformanceFactor, int callCount)
		{
			var target = new EventTarget();
			var sw = new Stopwatch();

			var expectedCalls = attachCount * callCount;

			for (int i = 0; i < attachCount; i++)
			{
				target.OrdinaryEvent += Target_OrdinaryEvent;
				target.WeakEvent += Target_OrdinaryEvent;
			}

			sw.Reset();
			sw.Start();
			for (int i = 0; i < callCount; i++)
			{
				target.RaiseOrdinaryEvent();
			}
			var msOrdinary = sw.ElapsedMilliseconds;
			Assert.That(target.CallCount, Is.EqualTo(expectedCalls));
			target.CallCount = 0;
			var meanOrdinary = msOrdinary / (float)callCount;

			sw.Reset();
			sw.Start();
			for (int i = 0; i < callCount; i++)
			{
				target.RaiseWeakEvent();
			}
			var msWeak = sw.ElapsedMilliseconds;
			Assert.That(target.CallCount, Is.EqualTo(expectedCalls));
			target.CallCount = 0;
			var meanWeak = msWeak / (float)callCount;

			Console.Out.WriteLine($"{nameof(EventTarget.OrdinaryEvent)} {callCount}calls; ela: {msOrdinary}ms; mean: {meanOrdinary}ms");
			Console.Out.WriteLine($"{nameof(EventTarget.WeakEvent)} {callCount}calls; ela: {msWeak}ms; mean: {meanWeak}ms");

			Assert.That(msOrdinary, Is.LessThanOrEqualTo(msWeak), "ordinary should be the fastest");

			if (meanWeak > 0f)
			{
				var p = ((100 / meanOrdinary) * meanWeak);
				Console.Out.WriteLine($"{nameof(meanWeak)} execution time is {p}% relative to {nameof(meanOrdinary)}");
			}

			Assert.That(msWeak / msOrdinary, Is.LessThanOrEqualTo(expectedPerformanceFactor), $"Performance factor {expectedPerformanceFactor} exceeded.");
		}

		private void Target_OrdinaryEvent(object sender, EventArgs e)
		{
			((EventTarget)sender).CallCount++;
		}

		private class EventTarget
		{
			public int CallCount = 0;

			readonly WeakEvent<EventHandler> _weakEvent = new WeakEvent<EventHandler>();
			public event EventHandler WeakEvent
			{
				add { _weakEvent.Add(value); }
				remove { _weakEvent.Remove(value); }
			}

			public event EventHandler<EventArgs> OrdinaryEvent;

			private readonly WeakEvent<AsyncEventHandler> _testEvent = new WeakEvent<AsyncEventHandler>();

			public event AsyncEventHandler TestEvent
			{
				add { _testEvent.Add(value); }
				remove { _testEvent.Remove(value); }
			}

			public void RaiseWeakEvent()
			{
				_weakEvent.Invoke(this, EventArgs.Empty);
			}

			public void RaiseOrdinaryEvent()
			{
				var handler = OrdinaryEvent;
				if (handler != null)
					handler(this, EventArgs.Empty);
			}
		}
	}
}
