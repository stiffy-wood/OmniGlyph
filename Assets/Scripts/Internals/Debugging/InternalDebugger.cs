using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat.Field;
using OmniGlyph.Configs;
using OmniGlyph.Internals.Events;
using UnityEngine;

namespace OmniGlyph.Internals.Debugging {
    public class InternalDebugger : IInternal {
        private DebugDrawer _drawer;
        private InputManager _inputManager;
        private InternalEventManager _eventManager;
        private TempStorage _tempStorage;
        public bool IsFinished { get; set; }

        private InternalsManager _manager;
        public void Init(InternalsManager manager) {
            _manager = manager;
            _drawer = _manager.GetMono<DebugDrawer>();
            _inputManager = _manager.Get<InputManager>();
            _eventManager = _manager.Get<InternalEventManager>();
            _tempStorage = _manager.Get<TempStorage>();
        }
        public void StartVisualizingSector(Action<Action> stopWatchingEventSubscriber, Sector sector) {
            if (!InternalConfig.IsDebug) {
                return;
            }
            GameObject sectorVisualizer = _drawer.SpawnDebugSector(sector.Center, new Vector3(sector.Dimensions.x * 0.9f, 0.3f, sector.Dimensions.z));
            Debug.Log($"Started visualizing sector {sector.Center}, GameObject: {sectorVisualizer}");

            stopWatchingEventSubscriber(() => {
                _drawer.DestroyDebugObject(sectorVisualizer);
            });
        }

        public void RegisterDebugInputEvent(ICondition inputCondition, Action onAction, Action offAction, bool startsOn) {
            if (!InternalConfig.IsDebug) {
                return;
            }
            Guid tempAddress = _tempStorage.Set(startsOn);
            _inputManager.StartListeningForInput(inputCondition, () => {
                Action[] actions = new Action[] { onAction, offAction };
                if (_tempStorage.Get<bool>(tempAddress)) {
                    _tempStorage.Set(false, tempAddress);
                    actions[0].Invoke();
                } else {
                    _tempStorage.Set(true, tempAddress);
                    actions[1].Invoke();
                }
            });
        }
        public void Log(string message) {
            if (InternalConfig.IsDebug) {
                Debug.Log(message);
            }
        }
        public void LogWarn(string message) {
            if (InternalConfig.IsDebug) {
                Debug.LogWarning(message);
            }
        }
        public void LogError(string message) {
            if (InternalConfig.IsDebug) {
                Debug.LogError(message);
            }
        }
        public void InternalUpdate() {

        }
    }
}
