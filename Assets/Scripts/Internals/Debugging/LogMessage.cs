using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals.Debugging {
    public class LogMessage : MonoBehaviour {
        public LogLevel logLevel;
        public string message;
        public DateTime time;
        public LogMessage(string message, LogLevel level) {
            this.logLevel = level;
            this.message = message;
            time = DateTime.Now;
        }
        public override string ToString() {
            return $"{time.ToString()} | [{logLevel.ToString().ToUpper()}]{new string(' ', 10 - logLevel.ToString().Length)}| {message}";

        }

    }
}
