using System;
using System.Collections.Generic;
using OmniGlyph.Configs;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.UI;
using UnityEngine;

namespace OmniGlyph.Internals {
    public abstract class OmniMono : MonoBehaviour {
        private InternalDebugger _debugger;
        protected InternalDebugger Debugger {
            get {
                if (_debugger == null) {
                    _debugger = gameObject.AddComponent<InternalDebugger>();
                }
                return _debugger;
            }
        }
        private GameContext _ctx;
        public GameContext Context {
            get {
                return _ctx;
            }
            protected set {
                _ctx = value;
            }
        }
    }
}

