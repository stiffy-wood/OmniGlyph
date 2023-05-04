using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace OmniGlyph.Internals {
    public delegate void KeysPressed(Guid EventGuid, KeyCode[] keys);
    public class InputManager : Input, IInternal {
        private bool _isFinished = false;
        public bool IsFinished {
            get {
                return _isFinished;
            }
        }
        public InternalsManager InternalsManager { get; set; }




        private InternalEventManager _internalEventManager;

        private Queue<Action> newIncomingEventRequests = new Queue<Action>();
        // TODO: Change implementation, so that it doesn't create a new event for each key press
        // It will create an event to get all pressed keys
        public void Update() {
            if (_internalEventManager == null) {
                _internalEventManager = InternalsManager?.Get<InternalEventManager>();
            } else {
                while (newIncomingEventRequests.Count > 0) {
                    newIncomingEventRequests.Dequeue().Invoke();
                }
            }
        }
        public void StartListeningForKey(KeyCode key, Action action, Action<DynamicEvent> callBack = null, InputTypes inputType = InputTypes.KeyHeld) {
            newIncomingEventRequests.Enqueue(() => {
                DynamicEvent @event = _internalEventManager.RegisterEvent(new InputCondition(new KeyCode[] { key }, inputType), action);
                callBack?.Invoke(@event);
            });
        }
        public void StopListetingForKey(DynamicEvent @event) {
            StopListeningForCombo(@event);
        }
        public void StartListeningForCombo(KeyCode[] keys, Action action, Action<DynamicEvent> callBack = null, InputTypes inputType = InputTypes.KeyHeld) {
            newIncomingEventRequests.Enqueue(() => {
                DynamicEvent @event = _internalEventManager.RegisterEvent(new InputCondition(keys, inputType), action);
                callBack?.Invoke(@event);
            });
        }
        public void StopListeningForCombo(DynamicEvent @event) {
            newIncomingEventRequests.Enqueue(() => _internalEventManager.UnRegisterEvent(@event));
        }


    }
}
