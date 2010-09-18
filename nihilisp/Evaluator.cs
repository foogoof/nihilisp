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
using System.Reflection;
using Foognostic.Nihilisp.Exceptions;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class Evaluator {
                public Evaluator() {
                }

                // FIXME TODO WARNING
                // This whole method sucks and will be ruthlessly refactored
                public object Evaluate(object form) {
                    if (form == null) {
                        throw new ArgumentNullException("No form supplied!");
                    }

                    if (form.GetType() != typeof(NLList)) {
                        return form.ToString();
                    }

                    NLList items = (NLList)form;
                    if (0 >= items.Contents.Length) {
                        throw new EvaluatorException("Meaningless to evaluate the empty list!");
                    }

                    NLSymbol symbol = (NLSymbol)items.Contents[0];
                    MethodInfo methodInfo = null;
                    object[] args = {};

                    if  (items.Contents.Length > 1) {
                        args = new object[items.Contents.Length - 1];
                        // FIXME: Seriously, me?
                        for (int i = 1; i < items.Contents.Length; i++) {
                            args[i - 1] = items.Contents[i];
                        }
                    }

                    object[] callingArgs = new object[args.Length];

                    // FIXME: This feels fragile
                    string ns = symbol.Namespace;
                    Type type = Type.GetType(ns);
                    if (type == null) {
                        throw new EvaluatorException("No type found for " + ns);
                    }

                    string funcName = symbol.FunctionName;
                    bool matchedOnName = false;
                    // FIXME: .Where() causing a compile error, works at the REPL?
                    for (int i = type.GetMethods().Length - 1; i >= 0; i--) {
                        MethodInfo info = type.GetMethods()[i];
                        bool paramTypeMismatch = false;

                        if (info.Name != funcName) {
                            continue;
                        }
                        matchedOnName = true;
                        if (info.GetParameters().Length != args.Length) {
                            continue;
                        } else if (info.GetParameters().Length == 0) {
                            methodInfo = info;
                            break;
                        }

                        for (int j = 0; j < args.Length; j++) {
                            ParameterInfo param = info.GetParameters()[j];
                            object formArg = args[j];

                            if (param == null || formArg == null) {
                                paramTypeMismatch = true;
                                break;
                            }

                            Type pt = param.ParameterType;
                            // FIXME: This is another good place to start decrapping.
                            // Need a good way to compare incoming forms with actual method signatures
                            if (pt == typeof(System.String)) {
                                string str = (string)formArg;
                                callingArgs[j] = str;
                                methodInfo = info;
                            }
                        }

                        if (paramTypeMismatch) {
                            continue;
                        }
                    }

                    if (methodInfo != null) {
                        // System.Console.WriteLine("*** about to invoke " + methodInfo.ToString() + " with " + callingArgs.ToString());
                        methodInfo.Invoke(null, callingArgs);
                    } else {
                        if (!matchedOnName) {
                            throw new EvaluatorException("Couldn't find a function by that name");
                        }
                        throw new EvaluatorException("Couldn't match args!");
                    }

                    return form;
                }
            }
        }
    }
}

