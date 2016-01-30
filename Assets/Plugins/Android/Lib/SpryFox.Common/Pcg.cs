/*
 * PCG Random Number Generation for C#.
 *
 * Copyright 2015 Kevin Harris <kevin@studiotectorum.com>
 * Copyright 2014 Melissa O'Neill <oneill@pcg-random.org>
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * For additional information about the PCG random number generation scheme,
 * including its license and other licensing options, visit
 *
 *     http://www.pcg-random.org
 */
namespace SpryFox.Common {
    public struct Pcg : RandomGenerator {
        #region Private internal state
        /// <summary>
        /// The RNG state. All values are possible.
        /// </summary>
        ulong m_state;

        /// <summary>
        /// Controls which RNG sequence (stream) is selected.
        /// Must <strong>always</strong> be odd.
        /// </summary>
        ulong m_inc;
        #endregion

        public Pcg(ulong shortSeed) {
            m_state = 0; m_inc = 0; // required syntactically
            Seed(shortSeed, shortSeed ^ 0xda3e39cb94b95bdbULL);
        }

        public Pcg(int seedA, int seedB, int seedC, int seedD) {
            m_state = 0; m_inc = 0; // required syntactically
            ulong initState = (((ulong)seedA) | (((ulong)seedB) << 32)) ^ 0xda3e39cb94b95bdbULL;
            ulong initSeq = (((ulong)seedC) | (((ulong)seedD) << 32)) ^ 0x853c49e6748fea9bULL;
            Seed(initState, initSeq);
        }

        public Pcg(int seedA, int seedB) {
            m_state = 0; m_inc = 0; // required syntactically
            ulong initState = ((ulong)seedA) ^ 0xda3e39cb94b95bdbULL;
            ulong initSeq = ((ulong)seedB) ^ 0x853c49e6748fea9bULL;
            Seed(initState, initSeq);
        }

        public Pcg(int superShortSeed) {
            m_state = 0; m_inc = 0; // required syntactically
            ulong seed = (ulong)superShortSeed | ((ulong)superShortSeed << 32);
            Seed(seed, seed ^ 0xda3e39cb94b95bdbULL);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pcg"/> class.
        /// </summary>
        /// <param name="initState">Initial state.</param>
        /// <param name="initSeq">Initial sequence</param>
        public Pcg(ulong initState, ulong initSeq) {
            m_state = 0; m_inc = 0; // required syntactically
            Seed(initState, initSeq);
        }

        /// <summary>
        /// Seed Pcg in two parts, a state initializer
        /// and a sequence selection constant (a.k.a.
        /// stream id).
        /// </summary>
        /// <param name="initState">Initial state.</param>
        /// <param name="initSeq">Initial sequence</param>
        public void Seed(ulong initState, ulong initSeq) {
            m_state = 0U;
            m_inc   = (initSeq << 1) | 1UL;
            GetNextUint();
            m_state += initState;
            GetNextUint();
        }

        /// <summary>
        /// Generates a uniformly-distributed 32-bit random number.
        /// </summary>
        public uint GetNextUint() {
            ulong oldState = m_state;
            m_state = oldState * 6364136223846793005ULL + m_inc;
            uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            return (xorShifted >> rot) | (xorShifted << ((-rot) & 31));
        }

        /// <summary>
        /// Generates a uniformly distributed number, r,
        /// where 0 <= r < exclusiveBound.
        /// </summary>
        /// <param name="exlusiveBound">Exlusive bound.</param>
        public uint GetNext(uint exclusiveBound) {
            // To avoid bias, we need to make the range of the RNG
            // a multiple of bound, which we do by dropping output
            // less than a threshold. A naive scheme to calculate the
            // threshold would be to do
            //
            //     uint threshold = 0x100000000UL % exclusiveBound;
            //
            // but 64-bit div/mod is slower than 32-bit div/mod 
            // (especially on 32-bit platforms). In essence, we do
            //
            //     uint threshold = (0x100000000UL - exclusiveBound) % exclusiveBound;
            //
            // because this version will calculate the same modulus,
            // but the LHS value is less than 2^32.
            uint threshold = (uint)((0x100000000UL - exclusiveBound) % exclusiveBound);

            // Uniformity guarantees that this loop will terminate.
            // In practice, it should terminate quickly; on average
            // (assuming all bounds are equally likely), 82.25% of
            // the time, we can expect it to require just one 
            // iteration. In the worst case, someone passes a bound
            // of 2^31 + 1 (i.e., 2147483649), which invalidates
            // almost 50% of the range. In practice bounds are
            // typically small and only a tiny amount of the range
            // is eliminated.
            for (;;) {
                uint r = GetNextUint();
                if (r >= threshold) {
                    return r % exclusiveBound;
                }
            }
        }

        public int GetNext(int exclusiveBound) {
            if(exclusiveBound <=0) throw new System.ArgumentException("No negatives allowed for PCG bound, got " + exclusiveBound);

            var ubound = (uint)exclusiveBound;
            int threshold = (int)((0x80000000L - exclusiveBound) % exclusiveBound);
            for (;;) {
                uint r = GetNextUint();
                if (r >= threshold) {
                    return (int)(r % ubound);
                }
            }
        }

        /// <summary>
        /// Generates a uniformly distributed number, r,
        /// where minimum <= r < exclusiveBound.
        /// </summary>
        /// <param name="minimum">The minimum inclusive value.</param>
        /// <param name="exclusiveBound">The maximum exclusive bound.</param>
        public int GetNext(int minimum, int exclusiveBound) {
            uint boundRange = (uint)(exclusiveBound - minimum);
            uint rangeResult = GetNext(boundRange);
            return (int)rangeResult + (int)minimum;
        }

        public float GetNextFloat() {
            return (float)GetNextUint()/uint.MaxValue;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="PcgRandom.Pcg"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="PcgRandom.Pcg"/>.</returns>
        public override string ToString() {
            return string.Format("[Pcg state: {0}; sequence: {1}]", m_state, m_inc);
        }
    }
}
