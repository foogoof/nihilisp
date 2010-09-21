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
using Foognostic.PRNG;

namespace Foognostic {
    namespace Nihilisp {
        namespace Test {

            [TestFixture()]
            public class TestMersenneTwister {
                [Test()]
                public void TestNoSeed() {
                    MersenneTwister mt = new MersenneTwister();
                    Assert.AreEqual(14514284786278117030, mt.GenRand());
                }

                [Test()]
                public void TestInitFromArray() {
                    MersenneTwister mt = new MersenneTwister(new ulong[] {0x12345, 0x23456, 0x34567, 0x45678});
                    Assert.AreEqual(7266447313870364031, mt.GenRand());
                }
            }
        }
    }
}

