// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details
using System;
using System.Diagnostics;
using System.Reflection;
using Amusoft.EventManagement.Compatibility;

namespace Amusoft.EventManagement
{
	internal static class HelperMethods
	{
		public static MethodInfo GetConsoleMethod(Type[] methodArguments)
		{
#if (NETSTANDARD)
			return CompatReflectionExtensions.GetRuntimeMethod(typeof(Debug), nameof(Debug.WriteLine), methodArguments);
#else
			return CompatReflectionExtensions.GetRuntimeMethod(typeof(Console), nameof(Console.WriteLine), methodArguments);
#endif
		}
	}
}