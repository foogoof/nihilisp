// //////////////////////////////////////////////////////////////////////////////
// Copyright 2010 Seth Schroeder
// This file is part of Nihilisp.
// 
// Nihilisp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nihilisp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Nihilisp.  If not, see <http://www.gnu.org/licenses/>.
// /////////////////////////////////////////////////////////////////////////////
// -*- mode: csharp -*-
// /////////////////////////////////////////////////////////////////////////////

using System;
using NUnit.Framework;
using Foognostic.Nihilisp.Core;

namespace Foognostic {
	namespace Nihilisp {
		namespace Test {
			[TestFixture()]
			public class TestReader
			{
				[Test()]
				public void TestSimpleInteger ()
				{
					long expected = 42, actual = 0;
					IForm form = new Reader().ReadFirstForm("42");
					Assert.IsNotNull(form);

					NLInteger nlint = (NLInteger)form;
					actual = nlint.val;
					Assert.AreEqual(expected, actual);
				}

				[Test()]
				public void TestSimpleString ()
				{
					string expected = "42", actual = "";
					IForm form = new Reader().ReadFirstForm("\"42\"");
					Assert.IsNotNull(form);

					NLString nlstr = (NLString)form;
					actual = nlstr.val;
					Assert.AreEqual(expected, actual);
				}
			}
		}
	}
}

