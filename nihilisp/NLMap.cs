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
            public class NLMap : NLClass, IForm, ISequence {

                private Dictionary<IForm, IForm> _val;

                public NLMap() {
                    _val = new Dictionary<IForm, IForm>();
                }

                public IForm[] val {
                    get {
                        IForm[] vals = new IForm[_val.Count * 2];
                        int i = 0;
                        foreach (IForm key in _val.Keys) {
                            vals[i++] = key;
                            vals[i++] = _val[key];
                        }
                        return vals;
                    }
                }

                public NLMap Assoc(IForm key, IForm val) {
                    _val[key] = val;
                    return this;
                }

                public IForm Get(IForm key) {
                    return _val[key];
                }

                public NLMap Dissoc(IForm key) {
                    _val.Remove(key);
                    return this;
                }

                public IForm[] Contents {
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

