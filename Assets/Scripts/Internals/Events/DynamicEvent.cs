using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Internals.Events {
    public record DynamicEvent {
        public ICondition condition;
        public Action action;
        public DynamicEvent(ICondition condition, Action action) {
            this.condition = condition;
            this.action = action;
        }
        public void Execute() {
            if (condition.Is()) {
                action();
            }
        }
        public override string ToString() {
            return $"DynamicEvent: \n\t{condition.ToString()}, \n\tAction:{nameof(action)}";
        }
    }
}
