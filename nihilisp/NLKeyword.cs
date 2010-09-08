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

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class NLKeyword : NLObject, IAtom, IForm {
                private string _val;
                public string val {
                    get { return _val; }
                }

                public static NLKeyword Create (string str) {
                    NLKeyword inst = new NLKeyword();
                    if (str.StartsWith(":")) {
                        inst._val = str;
                    } else {
                        inst._val = ':' + str;
                    }
                    return inst;
                }

                public IForm[] Contents {
                    get {
                        IForm[] ret = { this };
                        return ret;
                    }
                }

                public string Printable() {
                    return String.Format("#<[NLKeyword] [{0}]>", this._val);
                }
            }
        }
    }
}
