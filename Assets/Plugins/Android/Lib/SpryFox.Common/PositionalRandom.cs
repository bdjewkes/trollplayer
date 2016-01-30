using UnityEngine;

namespace SpryFox.Common {

    public struct PositionalRandom : RandomGenerator {
        int m_x, m_y, m_seed, m_incr;

        public PositionalRandom(int x, int y, int initialSeed) {
            m_x = x;
            m_y = y;
            m_seed = initialSeed;
            m_incr = 0;
        }

        public PositionalRandom(Vector2 pt, int initialSeed) {
            m_x = (int)pt.x;
            m_y = (int)pt.y;
            m_seed = initialSeed;
            m_incr = 0;
        }

        public float GetNextFloat() {
            var i = GetLargeInt(m_incr);
            m_incr = i;
            i = i & 0xfffff;
            return ((float)i) / 0xfffff;
        }

        // af: didn't test this, just chucked it in.
        public int GetNext(int upperBoundExc) {
            return GetLargeInt(m_incr) % upperBoundExc;
        }

        public float GetFloat(int localSeed) {
            int a = GetLargeInt(localSeed);
            a = a & 0xfffff;
            return ((float)a) / 0xfffff;
        }
        
        public int GetLargeInt(int localSeed) {
            // this is a variant of the Wang hash
            int totalSeed = (m_seed << 8) ^ localSeed;
            int a = (m_x + (m_y << 16)) + totalSeed;
            a = (a ^ 61) ^ (a >> 16);
            a = a ^ (totalSeed << 10);
            a = a + (a << 3);
            a = a ^ (a >> 4);
            a = a * 0x27d4eb2d;
            a = a ^ (a >> 15);
            return a;
        }

        public Vector2 InsideUnitCircle(int localSeed) {
            var angle = Mathf.PI * 2 * GetFloat(localSeed);
            var radius = Mathf.Sqrt(GetFloat(-localSeed));
            return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        }
    }


}