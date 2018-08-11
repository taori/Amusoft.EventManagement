// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under MIT
// See https://github.com/taori/Amusoft/blob/master/LICENSE.MD for details

#if NET35 || NET40
	
namespace System
{
	internal class WeakReference<T> where T : class
	{
		private readonly WeakReference _ref;

		/// <inheritdoc />
		public WeakReference(Object target)
		{
			_ref = new WeakReference(target);
		}

		public bool TryGetTarget(out T target)
		{
			var alive = _ref.IsAlive;
			target = _ref.Target as T;
			return alive;
		}
	}
}

#endif