using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals {
    public class DynamicEvent {
        public IICondition condition;
        public Action action;
        public DynamicEvent(IICondition condition, Action action) {
            this.condition = condition;
            this.action = action;
        }
        public void Execute() {
            if (condition.Is()) {
                action();
            }
        }
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            DynamicEvent e = (DynamicEvent)obj;
            return condition.Equals(e.condition) && action.Equals(e.action);
        }
        public override int GetHashCode() {
            int hash = 23;
            hash = hash ^ 31 + condition.GetHashCode();
            hash = hash ^ 31 + action.GetHashCode();
            return hash;
        }
    }
}
