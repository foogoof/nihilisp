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
using Foognostic.PRNG;

namespace Foognostic {
    namespace Nihilisp {
        namespace Test {

            [TestFixture()]
            public class TestReader {

                [Test()]
                public void TestSimpleInteger() {
                    long expected = 42, actual = 0;
                    object form = new Reader().ReadFirstForm("42");
                    Assert.IsNotNull(form);
                    actual = (long)form;
                    Assert.AreEqual(expected, actual);
                }

                [Test()]
                public void TestSimpleString() {
                    string expected = "42", actual = "";
                    object form = new Reader().ReadFirstForm("\"42\"");
                    Assert.IsNotNull(form);

                    actual = (string)form;
                    Assert.AreEqual(expected, actual);
                }

                [Test()]
                public void TestSimpleKeyword() {
                    string expected = ":foo", actual = "";
                    object form = new Reader().ReadFirstForm(":foo");
                    Assert.IsNotNull(form);

                    NLKeyword nlstr = (NLKeyword)form;
                    actual = nlstr.ToString();
                    Assert.AreEqual(expected, actual);
                }

                [Test()]
                public void TestVector() {
                    Reader rdr = new Reader();
                    NLVector vec = (NLVector)rdr.ReadFirstForm("[]");
                    Assert.IsNotNull(vec);
                    Assert.AreEqual(0, vec.Contents.Length);

                    vec = (NLVector) rdr.ReadFirstForm("[1]");
                    Assert.IsNotNull(vec);
                    Assert.AreEqual(1, vec.Contents.Length);
                    long ival = (long)vec.Contents[0];
                    Assert.AreEqual(1, ival);

                    vec = (NLVector)rdr.ReadFirstForm("[ 1, 2]");
                    Assert.IsNotNull(vec);
                    Assert.AreEqual(2, vec.Contents.Length);

                    ival = (long)vec.Contents[0];
                    Assert.AreEqual(1, ival);

                    ival = (long)vec.Contents[1];
                    Assert.AreEqual(2, ival);

                    vec = (NLVector)rdr.ReadFirstForm(" [ [, 42 ] \"a\" ]");
                    Assert.IsNotNull(vec);
                    Assert.AreEqual(2, vec.Contents.Length);

                    NLVector vval = (NLVector)vec.Contents[0];
                    Assert.AreEqual(42, (long)vval.Contents[0]);

                    string sval = (string)vec.Contents[1];
                    Assert.AreEqual("a", sval);
                }

                [Test()]
                public void TestList() {
                    Reader rdr = new Reader();
                    NLList list = (NLList)rdr.ReadFirstForm("( )");
                    Assert.IsNotNull(list);
                    Assert.AreEqual(0, list.Contents.Length);

                    list = (NLList)rdr.ReadFirstForm("(1)");
                    Assert.IsNotNull(list);
                    Assert.AreEqual(1, list.Contents.Length);
                    long ival = (long)list.Contents[0];
                    Assert.AreEqual(1, ival);

                    list = (NLList)rdr.ReadFirstForm("( 1, 2)");
                    Assert.IsNotNull(list);
                    Assert.AreEqual(2, list.Contents.Length);

                    ival = (long)list.Contents[0];
                    Assert.AreEqual(1, ival);

                    ival = (long)list.Contents[1];
                    Assert.AreEqual(2, ival);

                    list = (NLList)rdr.ReadFirstForm(" ( [, 42 ] \"a\" )");
                    Assert.IsNotNull(list);
                    Assert.AreEqual(2, list.Contents.Length);

                    NLVector vval = (NLVector)list.Contents[0];
                    Assert.AreEqual(42, (long)vval.Contents[0]);

                    string sval = (string)list.Contents[1];
                    Assert.AreEqual("a", sval);
                }

                [Test()]
                public void TestMap() {
                    Reader rdr = new Reader();
                    NLMap map = (NLMap)rdr.ReadFirstForm("{}");
                    Assert.AreEqual(0, map.Contents.Length);

                    map = (NLMap)rdr.ReadFirstForm("{ 42 \"everything\" }");
                    Assert.AreEqual(2, map.Contents.Length);
                    Assert.AreEqual(42, (long)map.Contents[0]);
                    Assert.AreEqual("everything", (string)map.Contents[1]);

                    map = (NLMap)rdr.ReadFirstForm("{ [0], :foo }");
                    Assert.AreEqual(2, map.Contents.Length);
                    Assert.AreEqual(0, (long)((NLVector)map.Contents[0]).Contents[0]);
                    Assert.AreEqual(":foo", ((NLKeyword)map.Contents[1]).ToString());

                    // Form equivalence not trivial
                    // NLMap map = (NLMap)form;
                    // Assert.AreEqual("everything", map.Get(NLInteger.Create("42")));
                }

                [Test()]
                public void TestSymbol() {
                    Reader rdr = new Reader();
                    NLSymbol sym = (NLSymbol)rdr.ReadFirstForm("foo");
                    Assert.AreEqual("foo", sym.ToString());

                    sym = (NLSymbol)rdr.ReadFirstForm("foo.bar");
                    Assert.AreEqual("foo", sym.Namespace);
                    Assert.AreEqual("bar", sym.FunctionName);
                }

                [Test()]
                public void TestMt() {
                    MersenneTwister mt = new MersenneTwister();
                    mt.GenRand();
                }
            }
        }
    }
}

