using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using Unity.VisualScripting;
using UnityEngine;

namespace OmniGlyph.Internals.Events {
    public class InternalEventManager : IInternal {

        private ConcurrentDictionary<DynamicEvent, byte> internalEvents = new ConcurrentDictionary<DynamicEvent, byte>();

        private bool _isFinished = false;
        public bool IsFinished {
            get {
                return _isFinished;
            }
        }
        public void Init(InternalsManager manager) {
            // Do nothing
        }
        public void InternalUpdate() {
            foreach (var conditionEventPair in internalEvents) {
                if (!conditionEventPair.Key.condition.Is()) {
                    continue;
                }
                conditionEventPair.Key.action.Invoke();
            }
        }

        public DynamicEvent GetEvent(InputCondition condition, Action action) {
            DynamicEvent newEvent = new DynamicEvent(condition, action);
            return GetEvent(newEvent);
        }
        public DynamicEvent GetEvent(DynamicEvent @event) {
            return internalEvents.Keys.Where(x => x.Equals(@event)).FirstOrDefault();

        }
        public DynamicEvent RegisterEvent(ICondition condition, Action registerAction) {
            DynamicEvent newEvent = new DynamicEvent(condition, registerAction);

            if (internalEvents.ContainsKey(newEvent)) {
                if (InternalConfig.IsDebug) {
                    Debug.LogWarning($"Event {newEvent} already exists");
                }
                return null;
            } else {
                if (!internalEvents.TryAdd(newEvent, 0)) {
                    throw new Exception($"Failed to add event {newEvent}");
                }
                if (InternalConfig.IsDebug) {
                    Debug.Log($"Added event {newEvent}");
                }
                return newEvent;
            }

        }
        public void UnRegisterEvent(DynamicEvent @event) {
            if (!internalEvents.ContainsKey(@event)) {
                return;
            }
            if (!internalEvents.TryRemove(@event, out _)) {
                throw new Exception($"Failed to remove event {@event}");
            }

            if (InternalConfig.IsDebug) {
                Debug.Log($"Removed event {@event}");
            }

        }
    }
}
