// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

using System;
using NUnit.Framework;
using System.Threading.Tasks;
using Amusoft.EventManagement.Test.TestEnvironment.EventReceiver;
using JetBrains.dotMemoryUnit;

[assembly:DotMemoryUnit(CollectAllocations = true, FailIfRunWithoutSupport = false)]
namespace Amusoft.EventManagement.Test.RetentionTests
{
	[TestFixture]
	public class OrdinaryRetentionTests
	{
		[Test]
		public void VerifyNoRentionWithNoSubscribers()
		{
			var checkBase = dotMemory.Check();

			var receiver = new EventHandlerReceiver();

			var check1 = dotMemory.Check(check =>
			{
				var objects = check.GetObjects(where => where.Type.Is(typeof(EventHandlerReceiver)));
				Assert.That(objects.ObjectsCount, Is.EqualTo(1));
			});

			receiver = null;
			GC.AddMemoryPressure(20_000_000);
			GC.Collect(2, GCCollectionMode.Forced, true);

			dotMemory.Check(check =>
			{
				var traffic = check.GetTrafficFrom(check1).Where(d => d.Type.Is(typeof(EventHandlerReceiver)));
				Assert.That(traffic.CollectedMemory.ObjectsCount, Is.GreaterThan(0));
			});
			dotMemory.Check(check =>
			{
				var objects = check.GetObjects(where => where.Type.Is(typeof(EventHandlerReceiver)));
				Assert.That(objects.ObjectsCount, Is.EqualTo(0));
			});
		}
	}
}
