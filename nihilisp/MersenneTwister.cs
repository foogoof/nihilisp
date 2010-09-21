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
    namespace PRNG {

        // Adapted from http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/VERSIONS/C-LANG/mt19937-64.c
        // See MersenneTwisterLicense.txt for details.
        // Transcription errors and efficiency losses regretted.
        public class MersenneTwister {
            private const int STATE_ITEMS = 312;
            private const int HALF_STATE_ITEMS = STATE_ITEMS / 2;
            private const ulong SEED = 5489;
            private const ulong YASEED = 6364136223846793005;
            private const ulong MATRIX_A = 0xB5026F5AA96619E9;
            private const ulong HIGH_ORDER_MASK = 0xFFFFFFFF80000000;
            private const ulong LOW_ORDER_MASK = 0x000000007FFFFFFF;
            private const ulong ARRAY_INIT_SEED_0 = 3935559000370003845;
            private const ulong ARRAY_INIT_SEED_1 = 2862933555777941757;

            private ulong[] _state;
            private ulong _pos;
            private int _seed;

            public MersenneTwister() {
                _pos = 1 + STATE_ITEMS;
                _seed = (int) SEED;
            }

            public MersenneTwister(DateTime dt) {
                _pos = 1 + STATE_ITEMS;
                _seed = dt.GetHashCode();
            }

            public MersenneTwister(ulong[] elts) {
                _pos = 1 + STATE_ITEMS;
                _seed = 19650218;
                init();
                InitFromArray(elts);
            }

            private void InitFromArray(ulong[] elts) {
                ulong prev, cur;
                int i = 1, j = 0, k = (STATE_ITEMS > elts.Length) ? STATE_ITEMS : elts.Length;
                
                for (; k > 0; k--) {
                    prev = _state[i - 1];
                    cur = _state[i];
                    prev ^= prev >> 62;
                    _state[i] = (cur ^ (prev * ARRAY_INIT_SEED_0)) + elts[j] + (ulong) j;
                    
                    if (++i >= STATE_ITEMS) {
                        _state[0] = _state[STATE_ITEMS - 1];
                        i = 1;
                    }
                    if (++j >= elts.Length) {
                        j = 0;
                    }
                }
                
                for (k = STATE_ITEMS - 1; k > 0; k--) {
                    prev = _state[i - 1];
                    cur = _state[i];
                    prev ^= prev >> 62;
                    _state[i] = (cur ^ (prev * ARRAY_INIT_SEED_1)) - (ulong) i;
                    
                    if (++i >= STATE_ITEMS) {
                        _state[0] = _state[STATE_ITEMS - 1];
                        i = 1;
                    }
                }
                
                _state[0] = 1uL << 63;
            }

            private void init() {
                _state = new ulong[STATE_ITEMS];
                _state[0] = (ulong) _seed;
                
                for (_pos = 1; _pos < STATE_ITEMS; _pos++) {
                    ulong prev = _state[_pos - 1];
                    _state[_pos] = ((prev >> 62) ^ prev) * YASEED + _pos;
                }
            }

            private void trace(int start, int elts, string label) {
                Console.Write("{0} nlt: ", label);
                for (int i = 0; i < elts; i++) {
                    Console.Write(String.Format("{0:d20} ", _state[start + i]));
                }
                Console.WriteLine();
            }

            public ulong GenRand() {
                int idx;
                ulong prval, hob, lob, tmp0, tmp1, tmp2;
                
                if (_pos >= STATE_ITEMS) {
                    if (_pos == 1 + STATE_ITEMS) {
                        init();
                    }
                    
                    for (idx = 0; idx < HALF_STATE_ITEMS; idx++) {
                        hob = _state[idx] & HIGH_ORDER_MASK;
                        lob = _state[idx + 1] & LOW_ORDER_MASK;
                        prval = hob | lob;
                        
                        tmp0 = _state[idx + HALF_STATE_ITEMS];
                        tmp1 = prval >> 1;
                        tmp2 = (prval % 2 == 0) ? 0 : MATRIX_A;
                        
                        _state[idx] = tmp0 ^ tmp1 ^ tmp2;
                    }
                    
                    for (; idx < STATE_ITEMS - 1; idx++) {
                        hob = _state[idx] & HIGH_ORDER_MASK;
                        lob = _state[idx + 1] & LOW_ORDER_MASK;
                        prval = hob | lob;
                        
                        tmp0 = _state[-HALF_STATE_ITEMS + idx];
                        tmp1 = prval >> 1;
                        tmp2 = (prval % 2 == 0) ? 0 : MATRIX_A;
                        
                        _state[idx] = tmp0 ^ tmp1 ^ tmp2;
                    }
                    
                    hob = HIGH_ORDER_MASK & _state[STATE_ITEMS - 1];
                    lob = LOW_ORDER_MASK & _state[0];
                    prval = hob | lob;
                    
                    tmp0 = _state[HALF_STATE_ITEMS - 1];
                    tmp1 = prval >> 1;
                    tmp2 = (prval % 2 == 0) ? 0 : MATRIX_A;
                    _state[STATE_ITEMS - 1] = tmp0 ^ tmp1 ^ tmp2;
                    
                    _pos = 0;
                }
                
                prval = _state[_pos++];
                
                prval ^= (prval >> 29) & 0x5555555555555555;
                prval ^= (prval << 17) & 0x71D67FFFEDA60000;
                prval ^= (prval << 37) & 0xFFF7EEE000000000;
                prval ^= (prval >> 43) & 0xFFFFFFFFFFFFFFFF;
                
                return prval;
            }

            // BEGIN: disgusting hacks for reflection invocation of methods from the REPL
            private static MersenneTwister mt;

            public static ulong GR() {
                if (mt == null) {
                    mt = new MersenneTwister();
                }
                ulong prval = mt.GenRand();
                System.Console.WriteLine("PRN: {0}, {1:x16}", prval, prval);
                
                return prval;
            }

            public static void GH() {
                ulong[] arr = { 0x12345, 0x23456, 0x34567, 0x45678 };
                MersenneTwister mt = new MersenneTwister(arr);
                for (int i = 0; i < 5; i++) {
                    Console.Write(String.Format("{0:d20} ", mt.GenRand()));
                }
                Console.WriteLine();
            }
            // END: disgusting hack for reflection invocation of methods from the REPL
        }
    }
}

