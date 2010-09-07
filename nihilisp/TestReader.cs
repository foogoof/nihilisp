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
            public class TestReader {

                [Test()]
                public void TestSimpleInteger() {
                    long expected = 42, actual = 0;
                    IForm form = new Reader().ReadFirstForm("42");
                    Assert.IsNotNull(form);

                    NLInteger nlint = (NLInteger)form;
                    actual = nlint.val;
                    Assert.AreEqual(expected, actual);
                }

                [Test()]
                public void TestSimpleString() {
                    string expected = "42", actual = "";
                    IForm form = new Reader().ReadFirstForm("\"42\"");
                    Assert.IsNotNull(form);

                    NLString nlstr = (NLString)form;
                    actual = nlstr.val;
                    Assert.AreEqual(expected, actual);
                }

                [Test()]
                public void TestSimpleKeyword() {
                    string expected = ":foo", actual = "";
                    IForm form = new Reader().ReadFirstForm(":foo");
                    Assert.IsNotNull(form);

                    NLKeyword nlstr = (NLKeyword)form;
                    actual = nlstr.val;
                    Assert.AreEqual(expected, actual);
                }

                [Test()]
                public void TestVector() {
                    Reader rdr = new Reader();
                    IForm form = rdr.ReadFirstForm("[]");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(0, form.Contents.Length);

                    form = rdr.ReadFirstForm("[1]");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(1, form.Contents.Length);
                    NLInteger ival = (NLInteger)form.Contents[0];
                    Assert.AreEqual(1, ival.val);

                    form = rdr.ReadFirstForm("[ 1, 2]");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(2, form.Contents.Length);

                    ival = (NLInteger)form.Contents[0];
                    Assert.AreEqual(1, ival.val);

                    ival = (NLInteger)form.Contents[1];
                    Assert.AreEqual(2, ival.val);

                    form = rdr.ReadFirstForm(" [ [, 42 ] \"a\" ]");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(2, form.Contents.Length);

                    NLVector vval = (NLVector)form.Contents[0];
                    Assert.AreEqual(42, ((NLInteger)vval.Contents[0]).val);

                    NLString sval = (NLString)form.Contents[1];
                    Assert.AreEqual("a", sval.val);
                }

                [Test()]
                public void TestList() {
                    Reader rdr = new Reader();
                    IForm form = rdr.ReadFirstForm("( )");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(0, form.Contents.Length);

                    form = rdr.ReadFirstForm("(1)");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(1, form.Contents.Length);
                    NLInteger ival = (NLInteger)form.Contents[0];
                    Assert.AreEqual(1, ival.val);

                    form = rdr.ReadFirstForm("( 1, 2)");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(2, form.Contents.Length);

                    ival = (NLInteger)form.Contents[0];
                    Assert.AreEqual(1, ival.val);

                    ival = (NLInteger)form.Contents[1];
                    Assert.AreEqual(2, ival.val);

                    form = rdr.ReadFirstForm(" ( [, 42 ] \"a\" )");
                    Assert.IsNotNull(form);
                    Assert.AreEqual(2, form.Contents.Length);

                    NLVector vval = (NLVector)form.Contents[0];
                    Assert.AreEqual(42, ((NLInteger)vval.Contents[0]).val);

                    NLString sval = (NLString)form.Contents[1];
                    Assert.AreEqual("a", sval.val);
                }

                [Test()]
                public void TestMap() {
                    Reader rdr = new Reader();
                    IForm form = rdr.ReadFirstForm("{}");
                    Assert.AreEqual(0, form.Contents.Length);

                    form = rdr.ReadFirstForm("{ 42 \"everything\" }");
                    Assert.AreEqual(2, form.Contents.Length);
                    Assert.AreEqual(42, ((NLInteger)form.Contents[0]).val);
                    Assert.AreEqual("everything", ((NLString)form.Contents[1]).val);

                    form = rdr.ReadFirstForm("{ [0], :foo }");
                    Assert.AreEqual(2, form.Contents.Length);
                    Assert.AreEqual(0, ((NLInteger)((NLVector)form.Contents[0]).Contents[0]).val);
                    Assert.AreEqual(":foo", ((NLKeyword)form.Contents[1]).val);

                    // Form equivalence not trivial
                    // NLMap map = (NLMap)form;
                    // Assert.AreEqual("everything", map.Get(NLInteger.Create("42")));
                }
            }
        }
    }
}

