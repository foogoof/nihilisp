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
using Foognostic.Nihilisp.Core;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class NLVector : NLClass, IForm, ISequence, IFlatCollection {
                public NLVector() {
                    _val = new IForm[0];
                    _list = new List<IForm>();
                }

                private IForm[] _val;
                private List<IForm> _list;

                public IForm[] val {
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

                public IFlatCollection Append(IForm form) {
                    _list.Add(form);
                    return this;
                }

                public IForm[] Contents {
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

