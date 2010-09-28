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
namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class NLSymbol {
                private string _val;

                public string val {
                    get { return _val; }
                }

                // FIXME: Wow this seems naive
                public string Namespace {
                    get {
                        if (_val == null || _val.Length == 0) {
                            return null;
                        }

                        int lastDot = _val.LastIndexOf('.');
                        if (-1 == lastDot) {
                            return null;
                        }

                        return _val.Substring(0, lastDot);
                    }
                }

                // FIXME: Wow this is not DRY
                public string FunctionName {
                    get {
                        if (_val == null || _val.Length == 0) {
                            return null;
                        }

                        int lastDot = _val.LastIndexOf('.');
                        if (-1 == lastDot || (1 + lastDot) == _val.Length) {
                            return _val;
                        }

                        return _val.Substring(1 + lastDot);
                    }
                }

                // FIXME: Why stop now?
                public string Assembly {
                    get {
                        string ns = this.Namespace;
                        if (ns == null || ns.Length == 0) {
                            return null;
                        }
                        int firstDot = ns.IndexOf('.');
                        if (-1 == firstDot || (1 + firstDot == ns.Length)) {
                            return null;
                        }
                        return ns.Substring(0, firstDot);
                    }
                }


                public NLSymbol() {
                }

                public static NLSymbol Create(string str) {
                    NLSymbol sym = new NLSymbol();
                    sym._val = str;
                    return sym;
                }

                public object[] Contents {
                    get {
                        object[] ret = { this };
                        return ret;
                    }
                }

                override public string ToString() {
                    return _val;
                }

                public string Printable() {
                    return String.Format("#<[NLSymbol] [{0}]>", this._val);
                }

            }
        }
    }
}

