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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Foognostic.Nihilisp.Exceptions;

namespace Foognostic {
    namespace Nihilisp {
        namespace Core {
            public class Reader {

                public Reader() {
                }

                public IForm ReadFirstForm(string text) {
                    return ReadNextForm(new StringReader(text));
                }

                public delegate IForm FormReader(TextReader stream, Reader reader);

                private FormReader[] FORM_READERS = {
                    StringFormReader, IntegerFormReader, KeywordFormReader,
                    VectorFormReader
                };

                public IForm ReadNextForm(TextReader stream) {
                    IForm form = null;
                    for (int i = 0; form == null && i < FORM_READERS.Length; i++) {
                        SkipWhitespace(stream);
                        if (Empty(stream)) {
                            return null;
                        }
                        form = FORM_READERS[i](stream, this);
                    }
                    return form;
                }

                public static IForm StringFormReader(TextReader stream, Reader reader) {
                    IForm form = null;
                    char cur = Convert.ToChar(stream.Peek());
                    if (cur != '"') {
                        return null;
                    }

                    bool escaping = false;
                    StringWriter buf = new StringWriter();

                    SkipChar(stream);
                    do {
                        if (Empty(stream)) {
                            throw new ReaderException("Invalid string literal");
                        }
                        cur = ReadChar(stream);
                        if (escaping) {
                            escaping = false;
                            if (!ESCAPE_SEQUENCE_MAP.ContainsKey(cur)) {
                                throw new ReaderException("Unexpected escape sequence \\" + cur);
                            }
                            cur = ESCAPE_SEQUENCE_MAP[cur];
                        }
                        else {
                            if (cur == '\\') {
                                escaping = true;
                            }
                            else if (cur == '"') {
                                if (!escaping) {
                                    form = NLString.Create(buf.ToString());
                                }
                            }
                        }

                        if (form == null && !escaping) {
                            buf.Write(cur);
                        }
                    } while (form == null);

                    return form;
                }

                public static IForm KeywordFormReader(TextReader stream, Reader reader) {
                    IForm form = null;
                    char cur = Convert.ToChar(stream.Peek());
                    if (cur != ':') {
                        return null;
                    }

                    SkipChar(stream);
                    StringWriter buf = null;

                    do {
                        if (Empty(stream)) {
                            if (buf == null) {
                                throw new ReaderException("Invalid keyword");
                            }
                            form = NLKeyword.Create(buf.ToString());
                        }
                        else {
                            cur = ReadChar(stream);
                            if (buf == null) {
                                // first character in symbol has more restrictions than subsequent chars
                                // TODO: clean this way the hell up
                                if (cur == '_' || !(new Regex(@"[\w\d]").Match(cur.ToString()).Success)) {
                                    throw new ReaderException("Invalid keyword");
                                }
                                buf = new StringWriter();
                                buf.Write(cur);
                            } else {
                                if (!(KEYWORD_PATTERN.Match(cur.ToString()).Success)) {
                                    throw new ReaderException("Invalid keyword");
                                }
                                buf.Write(cur);
                            }
                        }
                    } while (form == null);

                    return form;
                }

                // TODO: uh, handle negative numbers :-|
                // TODO: oh yeah, floats and doubles too.
                // TODO: hex! octal!
                // TODO: bonus points: variable radix (3r20 == 0x06)
                // TODO: ratios?
                public static IForm IntegerFormReader(TextReader stream, Reader reader) {
                    char cur = Convert.ToChar(stream.Peek());
                    if (!ISDIGIT_PATTERN.Match(cur.ToString()).Success) {
                        return null;
                    }

                    StringWriter buf = new StringWriter();
                    do {
                        buf.Write(ReadChar(stream));
                        if (Empty(stream)) {
                            break;
                        }
                        cur = Convert.ToChar(stream.Peek());
                    } while (ISDIGIT_PATTERN.Match(cur.ToString()).Success);

                    return NLInteger.Create(buf.ToString());
                }

                public static IForm VectorFormReader(TextReader stream, Reader reader) {
                    char cur = Convert.ToChar(stream.Peek());
                    if (cur != '[') {
                        return null;
                    }

                    NLVector vec = new NLVector();
                    SkipChar(stream);

                    do {
                        SkipWhitespace(stream);
                        IForm form = reader.ReadNextForm(stream);
                        if (form != null) {
                            vec.Append(form);
                        }
                        SkipWhitespace(stream);
                        if (Empty(stream)) {
                            throw new ReaderException("Unterminated vector!");
                        }

                    } while (PeekChar(stream, out cur) && cur != ']');

                    SkipChar(stream);

                    return vec;
                }

                public static bool Empty(TextReader stream) {
                    return stream == null || stream.Peek() == -1;
                }

                public static char ReadChar(TextReader stream) {
                    return Convert.ToChar(stream.Read());
                }

                public static bool PeekChar(TextReader stream, out char c) {
                    if (Empty(stream)) {
                        c = '\0';
                        return false;
                    }
                    c = Convert.ToChar(stream.Peek());
                    return true;
                }

                public static void SkipChar(TextReader stream) {
                    if (!Empty(stream)) {
                        stream.Read();
                    }
                }

                public static bool SkipWhitespace(TextReader stream) {
                    bool skippedAny = false;
                    char cur;
                    if (Empty(stream)) {
                        return false;
                    }
                    while (PeekChar(stream, out cur) &&
                           WHITESPACE_PATTERN.Match(cur.ToString()).Success) {
                        skippedAny = true;
                        SkipChar(stream);
                    }
                    return skippedAny;
                }

                private static Regex ISDIGIT_PATTERN = new Regex(@"\d");
                private static Regex KEYWORD_PATTERN = new Regex(@"[\w\d_]");
                private static Regex WHITESPACE_PATTERN = new Regex(@"[\s,]");

                private static Dictionary<char, char> ESCAPE_SEQUENCE_MAP = new Dictionary<char, char> {
                    { 'n', '\n' }, { '\\', '\\' }, { '"', '"' }
                };
            }
        }
    }
}

