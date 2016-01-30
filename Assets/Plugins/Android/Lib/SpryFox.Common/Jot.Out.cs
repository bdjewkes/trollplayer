using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

namespace SpryFox.Common {

    public partial class Jot {

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0>(T0 msg0) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1>(T0 msg0, T1 msg1) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2>(T0 msg0, T1 msg1, T2 msg2) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3>(T0 msg0, T1 msg1, T2 msg2, T3 msg3) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7, T8 msg8) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7); s_builder.Append(" "); 
            s_builder.Append(msg8);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7, T8 msg8, T9 msg9) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7); s_builder.Append(" "); 
            s_builder.Append(msg8); s_builder.Append(" "); 
            s_builder.Append(msg9);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7, T8 msg8, T9 msg9, T10 msg10) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7); s_builder.Append(" "); 
            s_builder.Append(msg8); s_builder.Append(" "); 
            s_builder.Append(msg9); s_builder.Append(" "); 
            s_builder.Append(msg10);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7, T8 msg8, T9 msg9, T10 msg10, T11 msg11) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7); s_builder.Append(" "); 
            s_builder.Append(msg8); s_builder.Append(" "); 
            s_builder.Append(msg9); s_builder.Append(" "); 
            s_builder.Append(msg10); s_builder.Append(" "); 
            s_builder.Append(msg11);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7, T8 msg8, T9 msg9, T10 msg10, T11 msg11, T12 msg12) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7); s_builder.Append(" "); 
            s_builder.Append(msg8); s_builder.Append(" "); 
            s_builder.Append(msg9); s_builder.Append(" "); 
            s_builder.Append(msg10); s_builder.Append(" "); 
            s_builder.Append(msg11); s_builder.Append(" "); 
            s_builder.Append(msg12);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }

        [System.Diagnostics.Conditional(s_cheapLogGuard)]
        public static void Out<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 msg0, T1 msg1, T2 msg2, T3 msg3, T4 msg4, T5 msg5, T6 msg6, T7 msg7, T8 msg8, T9 msg9, T10 msg10, T11 msg11, T12 msg12, T13 msg13) {
            s_builder.Length = 0;
            s_builder.Append(Time.time).Append(" ");
            s_builder.Append(msg0); s_builder.Append(" "); 
            s_builder.Append(msg1); s_builder.Append(" "); 
            s_builder.Append(msg2); s_builder.Append(" "); 
            s_builder.Append(msg3); s_builder.Append(" "); 
            s_builder.Append(msg4); s_builder.Append(" "); 
            s_builder.Append(msg5); s_builder.Append(" "); 
            s_builder.Append(msg6); s_builder.Append(" "); 
            s_builder.Append(msg7); s_builder.Append(" "); 
            s_builder.Append(msg8); s_builder.Append(" "); 
            s_builder.Append(msg9); s_builder.Append(" "); 
            s_builder.Append(msg10); s_builder.Append(" "); 
            s_builder.Append(msg11); s_builder.Append(" "); 
            s_builder.Append(msg12); s_builder.Append(" "); 
            s_builder.Append(msg13);
            if (s_isInEditor || !m_initialized || m_shouldPlayerLog) {
                Debug.Log(s_builder.ToString());
            }
            if(Callbacks != null) {
                Callbacks(s_builder.ToString(), "", LogType.Log);
            }
        }
    }
}