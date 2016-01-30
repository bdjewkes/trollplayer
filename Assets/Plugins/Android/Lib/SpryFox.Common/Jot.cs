using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpryFox.Common {
	// this logging class has convenience methods for logging multiple objects
    public partial class Jot {
        static bool m_initialized = false;
        static bool m_shouldPlayerLog = false;
        public static void Initialize(bool shouldPlayerLog = false) {
            if(m_initialized) return;
            m_shouldPlayerLog = shouldPlayerLog;
            Application.logMessageReceived += OnLog;
            s_isInEditor = Application.isEditor;
            m_initialized = true;
        }

        public static Application.LogCallback Callbacks;

        static void OnLog(string str, string stack, LogType type) {
            if (Callbacks != null) {
                Callbacks(str, stack, type);
            }
        }

        public static void SetMsgEnabled(string msgTag, bool enabled) {
            if(enabled) {
                s_enabledMsgs.Add(msgTag);
            } else {
                s_enabledMsgs.Remove(msgTag);
            }
        }

        public static bool GetMsgEnabled(string msgTag) {
            return s_enabledMsgs.Contains(msgTag);
        }

        private const string s_cheapLogGuard = "JOT_ENABLED";

        private static StringBuilder s_builder = new StringBuilder();

        static bool s_isInEditor = true;

        static HashSet<string> s_enabledMsgs = new HashSet<string>();

        // Out is a log function that is cheaper than Debug.Log (in players) because it doesn't capture stacks.
        // In the editor, it just uses Debug.Log, like a barbarian
    }

}