// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Amusoft.EventManagement.Compatibility
{
	internal static class TaskExtensions
	{
#if NET35 || NET40
		public static Task GetCompletedTask()
		{
			var tcs = new TaskCompletionSource<object>();
			tcs.SetResult(0);
			return tcs.Task;
		}
#else
		private static readonly Task CompletedTask = Task.FromResult(0);

		public static Task GetCompletedTask()
		{
			return CompletedTask;
		}
#endif

#if NET40
		public static Task WhenAll(IEnumerable<Task> source)
		{
			return TaskEx.WhenAll(source);
		}

		public static Task WhenAll(Task[] source)
		{
			return TaskEx.WhenAll(source);
		}
#else
		public static Task WhenAll(IEnumerable<Task> source)
		{
			return Task.WhenAll(source);
		}

		public static Task WhenAll(Task[] source)
		{
			return Task.WhenAll(source);
		}
#endif
	}
}