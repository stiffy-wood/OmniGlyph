using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals.Events {
    public record OnEventCondition : ICondition {
        private bool _triggered = false;
        public OnEventCondition(Action<Action> eventSubscription) {
            eventSubscription(() => _triggered = true);
        }
        public bool Is() {
            if (_triggered) {
                _triggered = false;
                return true;
            }
            return false;
        }
    }
}
