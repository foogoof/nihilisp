////////////////////////////////////////////////////////////////////////////////
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
////////////////////////////////////////////////////////////////////////////////
// -*- mode: csharp -*-
////////////////////////////////////////////////////////////////////////////////
using System;
using Foognostic.Nihilisp.Core;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class NLInteger : NLClass, IAtom, IForm {
                long _val;

                public long val {
                    get { return _val; }
                }

                public NLInteger () {
                }

                public static NLInteger Create (long val) {
                    NLInteger inst = new NLInteger ();
                    inst._val = val;
                    return inst;
                }

                public static NLInteger Create (string val) {
                    NLInteger inst = new NLInteger ();
                    inst._val = Int64.Parse(val);
                    return inst;
                }

                public IForm[] Contents {
                    get {
                        IForm[] ret = { this };
                        return ret;
                    }
                }

                public string Printable() {
                    return String.Format("#<[NLInteger] [{0}]>", this._val);
                }
            }
        }
    }
}
