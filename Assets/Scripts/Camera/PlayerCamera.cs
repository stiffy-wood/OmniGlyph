using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OmniGlyph.Actors;
using OmniGlyph.Combat;
using OmniGlyph.Internals;
using UnityEngine;

namespace OmniGlyph.Cam {
    public class PlayerCamera : OmniMono {
        [SerializeField]
        private Camera _cam;
        [SerializeField]
        private Vector3 _targetPos;
        [SerializeField]
        private Vector3 _targetLookPos;
        [SerializeField]
        private GameObject _focusObject;
        [SerializeField]
        private float _currentLag;
        public Camera Cam => _cam;
        void Start() {
            _cam = GetComponent<Camera>();
        }

        public override void Init(GameContext context) {
            base.Init(context);
            Context.GameStateChanged += OnGameStateChanged;
            InitFocusObject(Context.StateContext.CameraContext.FocusObjectTag);

        }
        private void Update() {
            if (_focusObject == null) {
                InitFocusObject(Context.StateContext.CameraContext.FocusObjectTag);
            }
            SetTargetPos(_focusObject.transform.position);
        }
        void FixedUpdate() {
            if (_focusObject == null) {
                return;
            }
            transform.position = Vector3.Lerp(transform.position, _targetPos, _currentLag);
            _targetLookPos = Vector3.Lerp(_targetLookPos, _focusObject.transform.position, _currentLag);
            transform.LookAt(_targetLookPos);
        }

        void SetTargetPos(Vector3 targetPos) {
            if (targetPos == _targetPos) {
                return;
            }
            _targetPos = targetPos + Context.StateContext.CameraContext.Offset;
            if (_currentLag == Context.StateContext.CameraContext.ChangeFocusLag) {
                _currentLag = Context.StateContext.CameraContext.Lag;
            }
        }
        void InitFocusObject(string objectTag) {
            _focusObject = GameObject.FindGameObjectWithTag(objectTag);
            if (_focusObject == null) {
                return;
            }
            Debugger.Log($"Focus object: {_focusObject}; {_focusObject.name}");
            if (_focusObject == null) {
                Debugger.ThrowCriticalError($"Could not find object with tag {Context.StateContext.CameraContext.FocusObjectTag}");
            }
            _currentLag = Context.StateContext.CameraContext.Lag;
            _targetPos = _focusObject.transform.position + Context.StateContext.CameraContext.Offset;
            _targetLookPos = _focusObject.transform.position;
        }
        void OnGameStateChanged(GameStates _) {
            Debugger.Log($"{nameof(PlayerCamera)}Game State Changed: {Context.CurrentGameState}");
            _focusObject = null;
            InitFocusObject(Context.StateContext.CameraContext.FocusObjectTag);
        }

    }
}
