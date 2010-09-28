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
using System.IO;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class NLVector : IFlatCollection {
                public NLVector() {
                    _val = new object[0];
                    _list = new List<object>();
                }

                private object[] _val;
                private List<object> _list;

                override public string ToString() {
                    StringWriter buf = new StringWriter();
                    buf.Write("[ ");
                    foreach (object o in val) {
                        buf.Write(String.Format("{0} ", PrettyPrinter.Reformat(o)));
                    }
                    buf.Write(']');
                    return buf.ToString();
                }

                public object[] val {
                    get {
                        int delta = _list.Count - _val.Length;
                        if (delta > 0) {
                            Array.Resize(ref _val, _val.Length + delta);
                            for (int i = _val.Length - delta; i < _val.Length; i++) {
                                _val[i] = _list[i];
                            }
                        }
                        return _val;
                    }
                }

                public IFlatCollection Append(object form) {
                    _list.Add(form);
                    return this;
                }

                public object[] Contents {
                    get {
                        return this.val;
                    }
                }

                public string Printable() {
                    return String.Format("#<[NLVector] [{0}]>", this.val.Length);
                }
            }
        }
    }
}

