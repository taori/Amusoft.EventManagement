// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details
using System;
using System.Reflection;
using Amusoft.EventManagement.Compatibility;

namespace Amusoft.EventManagement
{
	internal class WeakDelegate<THandler> where THandler : Delegate
	{
		/// <summary>
		/// Holder class for method + target information
		/// </summary>
		/// <param name="method">method</param>
		/// <param name="target">target</param>
		public WeakDelegate(MethodInfo method, object target)
		{
			_method = new WeakReference<MethodInfo>(method);
			_target = new WeakReference<object>(target);
		}

		/// <summary>
		/// Retrieves method and target information from their WeakReferences
		/// </summary>
		/// <param name="method">method</param>
		/// <param name="target">target</param>
		/// <returns></returns>
		public bool GetHandleInfo(out MethodInfo method, out object target)
		{
			target = null;
			return _method.TryGetTarget(out method) && (method.IsStatic || _target.TryGetTarget(out target));
		}

		private readonly WeakReference<MethodInfo> _method;

		private readonly WeakReference<object> _target;

		/// <summary>
		/// Checks if the handler is equivalent to the contained WeakReferences
		/// </summary>
		/// <param name="delegate">delegate</param>
		/// <returns>true if delegate is matching</returns>
		public bool IsMatch(THandler @delegate)
		{
			if (!_method.TryGetTarget(out var method))
				return false;

			if (method.IsStatic)
			{
				return ReferenceEquals(method, CompatReflectionExtensions.GetMethodInfo(@delegate));
			}
			else
			{
				if (!_target.TryGetTarget(out var target))
					return false;

				return ReferenceEquals(target, @delegate.Target)
				       && ReferenceEquals(method, CompatReflectionExtensions.GetMethodInfo(@delegate));
			}

		}
	}
}