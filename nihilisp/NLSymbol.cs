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
            public class NLSymbol : NLObject, IAtom, IForm {
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

                        int last_dot = _val.LastIndexOf('.');
                        if (-1 == last_dot) {
                            return null;
                        }

                        return _val.Substring(0, last_dot);
                    }
                }

                // FIXME: Wow this is not DRY
                public string FunctionName {
                    get {
                        if (_val == null || _val.Length == 0) {
                            return null;
                        }

                        int last_dot = _val.LastIndexOf('.');
                        if (-1 == last_dot || (1 + last_dot) == _val.Length) {
                            return null;
                        }

                        return _val.Substring(1 + last_dot);
                    }
                }

                // FIXME: Why stop now?
                public string Assembly {
                    get {
                        string ns = this.Namespace;
                        if (ns == null || ns.Length == 0) {
                            return null;
                        }
                        int first_dot = ns.IndexOf('.');
                        if (-1 == first_dot || (1 + first_dot == ns.Length)) {
                            return null;
                        }
                        return ns.Substring(0, first_dot);
                    }
                }


                public NLSymbol() {
                }

                public static NLSymbol Create(string str) {
                    NLSymbol sym = new NLSymbol();
                    sym._val = str;
                    return sym;
                }

                public IForm[] Contents {
                    get {
                        IForm[] ret = { this };
                        return ret;
                    }
                }

                public string Printable() {
                    return String.Format("#<[NLSymbol] [{0}]>", this._val);
                }

            }
        }
    }
}

