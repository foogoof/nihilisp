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
        namespace Exceptions {
            public class ReaderException : System.ApplicationException {
                public ReaderException() {}
                public ReaderException(string message) : base(message) { }
                public ReaderException(string message, System.Exception inner) : base(message, inner) {}

                protected ReaderException(System.Runtime.Serialization.SerializationInfo info,
                                          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            }

            public class EvaluatorException : System.ApplicationException {
                public EvaluatorException() {}
                public EvaluatorException(string message) : base(message) { }
                public EvaluatorException(string message, System.Exception inner) : base(message, inner) {}

                protected EvaluatorException(System.Runtime.Serialization.SerializationInfo info,
                                          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            }

            public class InternalException : System.ApplicationException {
                public InternalException() {}
                public InternalException(string message) : base(message) { }
                public InternalException(string message, System.Exception inner) : base(message, inner) {}

                protected InternalException(System.Runtime.Serialization.SerializationInfo info,
                                            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            }
        }
    }
}

