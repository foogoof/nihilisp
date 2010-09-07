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
                    VectorFormReader, MapFormReader, ListFormReader
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

                // ugh, still has some errors
                public static IForm KeywordFormReader(TextReader stream, Reader reader) {
                    char cur;
                    if (!(PeekChar(stream, out cur) && cur == ':')) {
                        return null;
                    }
                    SkipChar(stream);

                    // first char must also not be _ (for now)
                    if (!PeekChar(stream, out cur) ||
                        cur == '_' ||
                        !KEYWORD_PATTERN.Match(cur.ToString()).Success) {
                        throw new ReaderException("Invalid keyword syntax!");
                    }

                    StringWriter buf = new StringWriter();
                    buf.Write(ReadChar(stream));

                    IForm form = null;
                    do {
                        if (Empty(stream)) {
                            form = NLKeyword.Create(buf.ToString());
                        } else {
                            PeekChar(stream, out cur);
                            if (WHITESPACE_PATTERN.Match(cur.ToString()).Success) {
                                form = NLKeyword.Create(buf.ToString());
                                SkipWhitespace(stream);
                            } else if (KEYWORD_PATTERN.Match(cur.ToString()).Success) {
                                buf.Write(ReadChar(stream));
                            } else {
                                throw new ReaderException("Invalid keyword syntax");
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
                        PeekChar(stream, out cur);
                    } while (ISDIGIT_PATTERN.Match(cur.ToString()).Success);

                    return NLInteger.Create(buf.ToString());
                }

                // FIXME: this is probably not idiomatic
                public delegate IFlatCollection FormFactory();
                public static IFlatCollection VectorFactory() {
                    return new NLVector();
                }
                public static IFlatCollection ListFactory() {
                    return new NLList();
                }

                public static IFlatCollection FlatCollectionFormReader(TextReader stream, Reader reader, char open, char close, FormFactory factory) {
                    char cur;
                    if (!(PeekChar(stream, out cur) && cur == open)) {
                        return null;
                    }

                    IFlatCollection coll = factory();
                    SkipChar(stream);

                    do {
                        SkipWhitespace(stream);
                        IForm form = reader.ReadNextForm(stream);
                        if (form != null) {
                            coll.Append(form);
                        }
                        SkipWhitespace(stream);
                        if (Empty(stream)) {
                            throw new ReaderException("Unterminated vector!");
                        }

                    } while (PeekChar(stream, out cur) && cur != close);

                    SkipChar(stream);

                    return coll;
                }

                public static IForm VectorFormReader(TextReader stream, Reader reader) {
                    return (IForm) FlatCollectionFormReader(stream, reader, '[', ']', VectorFactory);
                }

                public static IForm ListFormReader(TextReader stream, Reader reader) {
                    return (IForm) FlatCollectionFormReader(stream, reader, '(', ')', ListFactory);
                }

                public static IForm MapFormReader(TextReader stream, Reader reader) {
                    char cur;
                    if (!(PeekChar(stream, out cur) && cur == '{')) {
                        return null;
                    }

                    NLMap map = new NLMap();
                    SkipChar(stream);

                    do {
                        IForm[] pair = { null, null };

                        SkipWhitespace(stream);
                        if (Empty(stream)) {
                            throw new ReaderException("Unterminated map!");
                        }

                        PeekChar(stream, out cur);
                        if (cur == '}') {
                            break;
                        }

                        pair[0] = reader.ReadNextForm(stream);
                        if (pair[0] == null) {
                            throw new ReaderException("Unreadable form in map!");
                        }

                        SkipWhitespace(stream);
                        if (Empty(stream)) {
                            throw new ReaderException("Unterminated map!");
                        }

                        pair[1] = reader.ReadNextForm(stream);
                        if (pair[1] == null) {
                            throw new ReaderException("Unreadable form in map!");
                        }

                        SkipWhitespace(stream);
                        if (Empty(stream)) {
                            throw new ReaderException("Unterminated map!");
                        }

                        map.Assoc(pair[0], pair[1]);
                    } while (PeekChar(stream, out cur) && cur != '}');

                    SkipChar(stream);

                    return map;
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

