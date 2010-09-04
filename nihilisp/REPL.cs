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

namespace Foognostic {
    namespace Nihilisp {
        namespace Runtime {
            public class REPL {
                public REPL () { }
                public static void Main() {
                    Reader arr = new Reader();

                    Console.WriteLine("Welcome to Nihilisp… probably best to abandon all hope ye who dwell here.\n");

                    do {
                        Console.Write("nilh> ");
                        string str = Console.ReadLine().Trim();
                        if (str.Length == 0) {
                            Console.WriteLine("");
                            continue;
                        }

                        try {
                            IForm form = arr.ReadFirstForm(str.Trim());
                            if (form == null) {
                                throw new Exception("uh, couldn't read " + str);
                            }
                            Console.WriteLine(form.Printable());
                        } catch (ReaderException rex) {
                            Console.WriteLine("Reader exception: " + rex.Message);
                            Console.WriteLine(rex.StackTrace);
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

