using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Internals;
using UnityEngine;

namespace OmniGlyph.Internals {
    public abstract class BaseMonoInternal : MonoBehaviour, IInternal {
        private bool _isFinished = false;
        public bool IsFinished {
            get {
                return _isFinished;
            }
        }
        protected InternalsManager _manager;
        public void InternalUpdate() {

        }
        public virtual void Init(InternalsManager manager) {
            _manager = manager;
        }
    }
}
