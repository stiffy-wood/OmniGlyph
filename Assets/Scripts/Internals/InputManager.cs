using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmniGlyph.Configs;
using OmniGlyph.Internals.Events;
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
        private InternalsManager _manager;

        private InternalEventManager _internalEventManager;

        public void Init(InternalsManager manager) {
            _manager = manager;
            _internalEventManager = _manager.Get<InternalEventManager>();
        }
        public void InternalUpdate() {

        }
        public DynamicEvent StartListeningForKey(KeyCode key, Action action, InputTypes inputType = InputTypes.KeyHeld) {
            return StartListeningForInput(new InputCondition(new KeyCode[] { key }, inputType), action);
        }
        public void StopListetingForKey(DynamicEvent @event) {
            StopListeningForCombo(@event);
        }
        public DynamicEvent StartListeningForCombo(KeyCode[] keys, Action action, InputTypes inputType = InputTypes.KeyHeld) {
            return StartListeningForInput(new InputCondition(keys, inputType), action);
        }
        public void StopListeningForCombo(DynamicEvent @event) {
            StopListeningForInput(@event);
        }
        public DynamicEvent StartListeningForInput(ICondition condition, Action action) {
            return _internalEventManager.RegisterEvent(condition, action);
        }
        public void StopListeningForInput(DynamicEvent @event) {
            _internalEventManager.UnRegisterEvent(@event);
        }


    }
}
