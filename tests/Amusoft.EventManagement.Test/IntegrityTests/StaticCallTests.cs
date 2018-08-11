// // Copyright 2018 Andreas Müller
// // This file is a part of Amusoft and is licensed under MIT
// // See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using System.Threading.Tasks;
using Amusoft.EventManagement.Test.TestEnvironment.EventSource;
using NUnit.Framework;

namespace Amusoft.EventManagement.Test.IntegrityTests
{
	[TestFixture]
	public class StaticCallTests
	{
		private class StaticCallInformation
		{
			public int CallCount { get; set; }
		}

		private class StaticCallReceiver
		{
			public static void Receive(object sender, StaticCallInformation args)
			{
				args.CallCount++;
			}
		}

		private class WeakStaticCallSource : IEventSource<EventHandler<StaticCallInformation>>
		{
			private WeakEvent<EventHandler<StaticCallInformation>> _event = new WeakEvent<EventHandler<StaticCallInformation>>();

			/// <inheritdoc />
			public void Add(EventHandler<StaticCallInformation> handler)
			{
				_event.Add(handler);
			}

			/// <inheritdoc />
			public void Remove(EventHandler<StaticCallInformation> handler)
			{
				_event.Remove(handler);
			}

			/// <inheritdoc />
			public EventHandler<StaticCallInformation> Invoker => (s, a) => _event.Invoke(s, a);
		}

		private class OrdinaryStaticCallSource : IEventSource<EventHandler<StaticCallInformation>>
		{
			private event EventHandler<StaticCallInformation> ReceiveEvent;

			/// <inheritdoc />
			public void Add(EventHandler<StaticCallInformation> handler)
			{
				ReceiveEvent += handler;
			}

			/// <inheritdoc />
			public void Remove(EventHandler<StaticCallInformation> handler)
			{
				ReceiveEvent -= handler;
			}

			/// <inheritdoc />
			public EventHandler<StaticCallInformation> Invoker => (s, a) => ReceiveEvent?.Invoke(s, a);
		}

		[Test]
		public async Task OrdinaryVerifyStaticCall()
		{
			var source = new OrdinaryStaticCallSource();
			source.Add(StaticCallReceiver.Receive);
			var information = new StaticCallInformation();
			source.Invoker(source, information);

			Assert.That(information.CallCount, Is.EqualTo(1));
		}

		[Test]
		public async Task WeakVerifyStaticCall()
		{
			var source = new WeakStaticCallSource();
			source.Add(StaticCallReceiver.Receive);
			var information = new StaticCallInformation();
			source.Invoker(source, information);

			Assert.That(information.CallCount, Is.EqualTo(1));
		}

		[Test]
		public async Task OrdinaryVerifyStaticUnsubscribedCall()
		{
			var source = new OrdinaryStaticCallSource();
			source.Add(StaticCallReceiver.Receive);
			source.Remove(StaticCallReceiver.Receive);
			var information = new StaticCallInformation();
			source.Invoker(source, information);

			Assert.That(information.CallCount, Is.EqualTo(0));
		}

		[Test]
		public async Task WeakVerifyStaticUnsubscribedCall()
		{
			var source = new WeakStaticCallSource();
			source.Add(StaticCallReceiver.Receive);
			source.Remove(StaticCallReceiver.Receive);
			var information = new StaticCallInformation();
			source.Invoker(source, information);

			Assert.That(information.CallCount, Is.EqualTo(0));
		}
	}
}