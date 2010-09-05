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
            public class NLList : NLClass, IForm, ISequence {

                List<IForm> forms;

                public NLList() {
                    forms = new List<IForm>();
                }

                public NLList append(IForm form) {
                    forms.Add(form);
                    return this;
                }

                public IForm[] Contents {
                    get {
                        IForm[] formArr = new IForm[forms.Count];
                        for (int i = 0; i < forms.Count; i++) {
                            formArr[i] = forms[i];
                        }
                        return formArr;
                    }
                }

                public string Printable() {
                    return String.Format("#<[NLList] [{0}]>", this.forms.Count);
                }
             }
        }
    }
}
