using System.Diagnostics;

/*
 * TimeBlock is a quick and cheap way of surfacing time related problems in
 * the game.  It's like an assert which alerts when the time taken exceeds
 * some expected value.
 *
 * To use, put TimeGuard.Block inside a using statement:
 *       using(TimeGuard.Block(5, "sample block")) {
 *           do_some_stuff();
 *       }
 *
 *
 * To handle yields, use the Untimed function like this:
 *       using(var tg = TimeGuard.Block(5, "sample block")) {
 *           do_some_stuff();
 *           using(tg.Untimed()) {
 *               yield return null;
 *           }
 *       }
 *
 * TimeGuards become no-ops if the preprocessor flag TIMEGUARD_ENABLED is unset.
 */

namespace SpryFox.Common {
    public struct TimeGuard : System.IDisposable {
        private const string s_timeFlag = "TIMEGUARD_ENABLED";

        /// <summary>Create a TimeGuard for a block of code.  The first
        /// argument is what's displayed if the block is exceeded, the second
        /// is the number of milliseconds we should expect the block to
        /// take.</summary>
        public static TimeGuard Block(float milliseconds, string name = "[unnamed]") {
#if TIMEGUARD_ENABLED
            var tg = new TimeGuard();
            tg.m_name = name;
            tg.m_ms = milliseconds;
            tg.m_stopwatch = Stopwatch.StartNew();
            return tg;
#else
            return new TimeGuard();
#endif      
        }

        /// Called at the end of a Using block.  This performs the time check.
        public void Dispose() {
#if TIMEGUARD_ENABLED
            m_stopwatch.Stop();
            if(MillisecondsTaken > m_ms) {
                Log.Out("TimeGuard for", m_name, "exceeded", m_ms, "ms:", MillisecondsTaken, "ms");
            }
#endif
        }

        /// <summary>Call this to stop timing for a block of code.  Makes the
        /// most sense to do this for yield statements.</summary>
        public UntimedBlock Untimed() {
            return new UntimedBlock(m_stopwatch);
        }

        /// <summary>Returns the fractional number of milliseconds that the
        /// timeguard has recorded thus far.</summary>

        public float MillisecondsTaken {
            get {
#if TIMEGUARD_ENABLED
                return 1000f * m_stopwatch.ElapsedTicks/Stopwatch.Frequency;
#else
                return 0;
#endif
            }
        }

        /// Strut to implement the untimed blocks.
        public struct UntimedBlock : System.IDisposable {
            public UntimedBlock(Stopwatch stopwatch) {
                m_stopwatch = stopwatch;
#if TIMEGUARD_ENABLED
                m_stopwatch.Stop();
#endif
            }

            public void Dispose() {
#if TIMEGUARD_ENABLED
                m_stopwatch.Start();
#endif
            }

            #pragma warning disable 0414
            Stopwatch m_stopwatch;
            #pragma warning restore 0414
        }

        string m_name;
        float m_ms;

        #pragma warning disable 0649
        Stopwatch m_stopwatch;
        #pragma warning restore 0649
    }
}