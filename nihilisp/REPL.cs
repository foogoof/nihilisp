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
using Foognostic.Nihilisp.Core;
using Foognostic.Nihilisp.Exceptions;
using Foognostic.Nihilisp.PRNG;

namespace Foognostic {
    namespace Nihilisp {
        namespace Runtime {
            public class REPL {
                public REPL () { }

                public static void Greet() {
                    string[] messages = {
                        "Is it meaningful? Doubtful…",
                        "Do (this) instead of this()",
                        "It may cause an alignment shift to Chaotic Neutral.",
                        "Anyone forcing you to use it may be evil in a better-for-them-than-you way.",
                        "Not worried about quality? Good."
                    };
                    Console.Write(String.Format("Welcome to Nihilisp! {0}\n\n",
                        messages[new MersenneTwister(DateTime.UtcNow).GenRand() % (ulong)messages.Length]));
                }

                public static void Main() {
                    Reader arr = new Reader();
                    Evaluator eval = new Evaluator();
                    Greet();
                    do {
                        Console.Write("nihil> ");

                        string str = Console.ReadLine().Trim();
                        // FIXME: the really annoying echo problem

                        if (str.Length == 0) {
                            // Why yes, this *is* the hard way of printing an empty line.
                            str = "(System.Console.WriteLine)";
                        }

                        try {
                            object form = arr.ReadFirstForm(str.Trim());
                            if (form == null) {
                                throw new Exception("uh, couldn't read " + str);
                            }
                            form = eval.Evaluate(form);
                            if (form != null) {
                                Console.WriteLine(PrettyPrinter.Reformat(form));
                            }
                        } catch (ReaderException rex) {
                            Console.WriteLine("Reader exception: " + rex.Message);
                            Console.WriteLine(rex.StackTrace);
                        } catch (EvaluatorException eex) {
                            Console.WriteLine("Evaluator exception: " + eex.Message);
                            Console.WriteLine(eex.StackTrace);
                        } catch (Exception ex) {
                            Console.WriteLine("UNEXPECTED exception: " + ex.Message);
                            Console.WriteLine(ex.StackTrace);
                        }
                    } while (true);
                }
            }
        }
    }
}

