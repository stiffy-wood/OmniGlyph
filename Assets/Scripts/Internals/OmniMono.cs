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
            private set {
                _ctx = value;
            }
        }
        // For controllers and other singletons initialized using GameContext
        public virtual void Init(GameContext context) {
            _ctx = context;
        }
        // For objects that are not initialized using GameContext
        public virtual void InternalInit() {
            _ctx = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameContext>();
        }
    }
}

