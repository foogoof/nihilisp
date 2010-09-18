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
using System.Collections.Generic;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class NLMap {

                private Dictionary<object, object> _val;

                public NLMap() {
                    _val = new Dictionary<object, object>();
                }

                public object[] val {
                    get {
                        object[] vals = new object[_val.Count * 2];
                        int i = 0;
                        foreach (object key in _val.Keys) {
                            vals[i++] = key;
                            vals[i++] = _val[key];
                        }
                        return vals;
                    }
                }

                public NLMap Assoc(object key, object val) {
                    _val[key] = val;
                    return this;
                }

                public object Get(object key) {
                    return _val[key];
                }

                public NLMap Dissoc(object key) {
                    _val.Remove(key);
                    return this;
                }

                public object[] Contents {
                    get {
                        return this.val;
                    }
                }

                public string Printable() {
                    return String.Format("#<[NLMap] [{0}]>", _val.Count);
                }
            }
        }
    }
}

