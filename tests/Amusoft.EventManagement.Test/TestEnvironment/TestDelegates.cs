// Copyright 2018 Andreas Müller
// This file is a part of Amusoft and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.EventManagement/blob/master/LICENSE.MD for details

using System;
using System.Threading.Tasks;

namespace Amusoft.EventManagement.Test.TestEnvironment
{
	public delegate void CustomVoidParametersHandler(string a, int b, int c);

	public delegate Task AsyncEventHandler(object sender, EventArgs empty);
}