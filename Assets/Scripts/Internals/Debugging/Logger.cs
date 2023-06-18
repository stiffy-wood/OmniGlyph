using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace OmniGlyph.Internals.Debugging {
    public static class Logger {
        private static ObservableList<LogMessage> _logs = new ObservableList<LogMessage>();
        public static ObservableList<LogMessage> Logs => _logs;

        private static void AddLog(string message, LogLevel level) {
            _logs.Add(new LogMessage(message, level));
        }

        public static void Log(object message) {
            Debug.Log(message);
            AddLog(message.ToString(), LogLevel.Info);
        }
        public static void LogWarn(object message) {
            Debug.LogWarning(message);
            AddLog(message.ToString(), LogLevel.Warning);
        }
        public static void LogError(object message) {
            Debug.LogError(message);
            AddLog(message.ToString(), LogLevel.Error);
        }
    }
}
