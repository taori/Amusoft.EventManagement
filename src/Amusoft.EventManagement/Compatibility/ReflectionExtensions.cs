// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amusoft.EventManagement.Compatibility
{
	internal static class CompatReflectionExtensions
	{
		public static MethodInfo GetMethodInfo(Delegate source)
		{
#if NET35 || NET40
			return source.Method;
#else
			return RuntimeReflectionExtensions.GetMethodInfo(source);
#endif
		}

		public static ConstructorInfo GetConstructorInfo(Type source, Type[] constructorParameterTypes)
		{
#if NETSTANDARD1_1
			foreach (var constructorInfo in source.GetTypeInfo().DeclaredConstructors)
			{
				var currentParams = constructorInfo.GetParameters();
				if(currentParams.Length != constructorParameterTypes.Length)
					continue;

				for (int i = 0; i < currentParams.Length; i++)
				{
					if (currentParams[i].ParameterType != constructorParameterTypes[i])
						continue;
				}

				return constructorInfo;
			}

			return null;
#else
			return source.GetConstructor(constructorParameterTypes);
#endif
		}

		public static MethodInfo GetRuntimeMethod(Type source, string name, params Type[] parameters)
		{
#if NET35 || NET40
			return source.GetMethod(name, parameters);
#else
			return RuntimeReflectionExtensions.GetRuntimeMethod(source, name, parameters);
#endif
		}

		public static PropertyInfo GetRuntimeProperty(Type source, string name)
		{
#if NET35 || NET40
			return source.GetProperty(name);
#else
			return RuntimeReflectionExtensions.GetRuntimeProperty(source, name);
#endif
		}

		public static MethodInfo GetGetMethod(this PropertyInfo source)
		{
#if NET35 || NET40
			return source.GetGetMethod();
#else
			return source.GetMethod;
#endif
		}

		public static IEnumerable<MethodInfo> GetRuntimeMethods(Type source)
		{
#if NET35 || NET40
			return source.GetMethods();
#else
			return source.GetRuntimeMethods();
#endif
		}
	}
}