using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Combat;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using OmniGlyph.Internals.Events;
using UnityEngine;
namespace OmniGlyph.Player {
    public class Player : MonoBehaviour, IActor {
        [SerializeField]
        CombatActor _combatActor;

        public event Action<Vector3> PositionChanged;
        public event Action<IActor> ActorDied;


        private event Action _playerInputEnabled;
        private event Action _playerInputDisabled;

        public Vector3 GetPosition() {
            return transform.position;
        }
        public void SetPosition(Vector3 targetPos) {
            _targetPos = targetPos;
            PositionChanged?.Invoke(_targetPos);
        }
        public CombatActor CombatActor {
            get {
                return _combatActor;
            }
        }
        public Vector3 GetSize() {
            return GetComponent<Collider>().bounds.size;
        }

        public float WalkSpeed = 0.005f;
        public float RunSpeedModifier = 2f;
        public float MovementLag = 0.3f;


        InternalsManager _internalsManager;
        InputManager _inputManager;
        InternalDebugger _debugger;
        List<DynamicEvent> _inputEvents;

        Vector3 _targetPos;

        BattleController _battleController;

        // Start is called before the first frame update
        void Start() {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            _internalsManager = controller.GetComponent<InternalsManager>();
            _battleController = controller.GetComponent<BattleController>();

            _inputManager = _internalsManager.Get<InputManager>();
            _inputEvents = new List<DynamicEvent>();
            _debugger = _internalsManager.Get<InternalDebugger>();

            if (InternalConfig.IsDebug) {
                _debugger.RegisterDebugInputEvent(
                    new InputCondition(new KeyCode[] { KeyCode.B }, InputTypes.KeyUp),
                    () => {
                        _battleController.CombatStart(new List<CombatActor> { _combatActor });
                        Debug.Log($"Pressed B, starting combat");
                    },
                    () => {
                        _battleController.CombatEnd();
                        Debug.Log($"Pressed B again, ending combat");
                    },
                    true);
            }

            StartListeningForInputKeys();

            _battleController.CombatStarted += OnCombatStart;
            _battleController.CombatEnded += OnCombatEnd;

            _combatActor = new CombatActor(100, 100, 10, this);
            _combatActor.startingRange = CombatRanges.Far;
            SetPosition(transform.position);
        }
        void StartListeningForInputKeys() {
            _inputEvents.AddRange(new DynamicEvent[] {
                _inputManager.StartListeningForKey(UserInputsConfig.KeyMoveForward, GetOnInputAction(Vector3.forward)),
                _inputManager.StartListeningForKey(UserInputsConfig.KeyMoveBackward, GetOnInputAction(Vector3.back)),
                _inputManager.StartListeningForKey(UserInputsConfig.KeyMoveLeftward, GetOnInputAction(Vector3.left)),
                _inputManager.StartListeningForKey(UserInputsConfig.KeyMoveRightward, GetOnInputAction(Vector3.right)),

            });
            _playerInputEnabled?.Invoke();
        }
        void StopListeningForInputKeys() {
            foreach (var @event in _inputEvents) {
                _inputManager.StopListetingForKey(@event);
            }
            _inputEvents.Clear();
            _playerInputDisabled?.Invoke();
        }
        void OnCombatStart(Vector3 battlefieldPos) {
            StopListeningForInputKeys();
        }
        void OnCombatEnd() {
            StartListeningForInputKeys();
        }
        Action GetOnInputAction(Vector3 direction) {
            return () => {
                SetPosition(_targetPos + direction * WalkSpeed * (InputManager.GetKey(UserInputsConfig.KeyMoveSprintward) ? RunSpeedModifier : 1));
            };
        }

        private void OnDestroy() {
            foreach (var @event in _inputEvents) {
                _inputManager.StopListetingForKey(@event);
            }
        }
        void FixedUpdate() {
            transform.position = Vector3.Lerp(transform.position, _targetPos, MovementLag);
        }
    }
}