//////////////////////////////////////////////////////////////////////////////
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
////////////////////////////////////////////////////////////////////////////////
// -*- mode: csharp -*-
////////////////////////////////////////////////////////////////////////////////


using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using Foognostic.Nihilisp.Exceptions;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class Reader {

                public Reader () {
                }

                public IForm ReadFirstForm(string text) {
                    return ReadNextForm(new StringReader(text));
                }

                public IForm ReadNextForm(TextReader stream) {
                    StringWriter buf = new StringWriter();

                    bool done = Empty(stream);
                    if (done) {
                        return null;
                    }
                    char cur = Convert.ToChar(stream.Peek());

                    if (cur == '"') {
                        bool escaping = false;
                        stream.Read(); // advance past initial quote
                        done = Empty(stream);
                        do {
                            if (!done) {
                                cur = Convert.ToChar(stream.Read());
                            }
                            if (escaping) {
                                escaping = false;
                                if (!escape_sequence_map.ContainsKey(cur)) {
                                    throw new ReaderException("Unexpected escape sequence \\" + cur);
                                }
                                cur = escape_sequence_map[cur];
                            }
                            else {
                                if (cur == '\\') {
                                    escaping = true;
                                }
                                else if (cur == '"') {
                                    if (!escaping) {
                                        return NLString.Create(buf.ToString());
                                    }
                                } else if (done) {
                                    throw new ReaderException("Incomplete string literal: " + buf.ToString());
                                }
                            }

                            if (!escaping) {
                                buf.Write(cur);
                            }
                        } while (true);
                    }
                    // TODO: uh, handle negative numbers :-|
                    // TODO: oh yeah, floats and doubles too.
                    // TODO: hex! octal!
                    // TODO: bonus points: variable radix (3r20 == 0x06)
                    // TODO: ratios?
                    else if (isdigit_pattern.Match(cur.ToString()).Success) {
                        buf.Write(Convert.ToChar(stream.Read()));
                        do {
                            done = Empty(stream) || !isdigit_pattern.Match(Convert.ToString(stream.Peek())).Success;
                            if (done) {
                                return NLInteger.Create(buf.ToString());
                            }
                            buf.Write(Convert.ToChar(stream.Read()));
                        } while (true);
                    }
                    return null;
                }

                private static bool Empty(TextReader stream) {
                    return stream == null || stream.Peek() == -1;
                }

                private static Regex isdigit_pattern = new Regex(@"\d");

                private static Dictionary<char, char> escape_sequence_map = new Dictionary<char, char>() {
                    { 'n', '\n' },
                    { '\\', '\\' },
                    { '"', '"' }
                };

            }
        }
    }
}

