using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Internals;
using Unity.VisualScripting;
using UnityEngine;

namespace OmniGlyph.Internals {
    public class InternalEventManager : IInternal {
        private HashSet<DynamicEvent> internalEvents = new HashSet<DynamicEvent>();

        private bool _isFinished = false;
        public bool IsFinished {
            get {
                return _isFinished;
            }
        }
        public InternalsManager InternalsManager { get; set; }

        public void Update() {
            foreach (DynamicEvent @event in internalEvents) {
                @event.Execute();
            }
        }

        public DynamicEvent GetEvent(InputCondition condition, Action action) {
            DynamicEvent newEvent = new DynamicEvent(condition, action);
            return GetEvent(newEvent);
        }
        public DynamicEvent GetEvent(DynamicEvent @event) {
            internalEvents.TryGetValue(@event, out DynamicEvent e);
            return e;
        }
        public DynamicEvent RegisterEvent(IICondition condition, Action action) {
            DynamicEvent newEvent = new DynamicEvent(condition, action);

            if (internalEvents.Add(newEvent)) {
                return newEvent;
            }
            return GetEvent(newEvent);
        }
        public void UnRegisterEvent(DynamicEvent @event) {
            internalEvents.Remove(@event);
        }
    }
}
