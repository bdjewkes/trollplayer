using UnityEngine;
using System.Collections;

namespace SpryFox.Common {
    public struct Xorshift : RandomGenerator {
        public Xorshift(int seed) {
            x = Mix(Mix((uint)seed));
            y = Mix(362436069 ^ (uint)seed);
            z = Mix(521288629 ^ (uint)seed);
            w = 88675123;
        }

        public Xorshift(int seedA, int seedB, int seedC, int seedD) {
            x = Mix(Mix((uint)seedA));
            y = Mix((uint)seedB) ^ 362436069;
            z = Mix((uint)seedC) ^ 521288629;
            w = Mix((uint)seedD) ^ 88675123;
        }

        public Xorshift(uint seed) {
            x = Mix(Mix(seed));
            y = Mix(362436069 ^ (uint)seed);
            z = Mix(521288629 ^ (uint)seed);
            w = 88675123;
        }

        public static uint Mix(uint h) {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }

        public uint GetNextUint() {
            uint t;
            t = (x ^ (x << 11));
            x = y; y = z; z = w;
            return (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)));
        }

        public float GetNextFloat() {
            return (float)GetNextUint()/uint.MaxValue;
        }

        public int GetNext(int upperBoundExc) {
            uint divisor = (uint)(uint.MaxValue / upperBoundExc);
            uint retval = GetNextUint()/divisor;
            for(int i = 0; i < 10 && upperBoundExc <= retval; i++) {
                retval = GetNextUint()/divisor;
            }
            return (int)retval;
            //return (int)(GetNextUint() % upperBoundExc);
        }

        uint x, y, z, w;
    }
}