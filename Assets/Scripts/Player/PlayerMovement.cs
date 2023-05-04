using System;
using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Configs;
using OmniGlyph.Internals;
using UnityEngine;
namespace OmniGlyph.Player {
    public class PlayerMovement : MonoBehaviour {
        public event Action<Vector3> PositionChanged;
        public float WalkSpeed = 0.005f;
        public float RunSpeedModifier = 2f;
        public float Lag = 0.3f;


        InternalsManager _internalsManager;
        InputManager _im;

        List<DynamicEvent> _inputEvents;

        Vector3 _targetPos;

        // Start is called before the first frame update
        void Start() {
            _internalsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<InternalsManager>();
            _im = _internalsManager.Get<InputManager>();
            _inputEvents = new List<DynamicEvent>();

            _im.StartListeningForKey(UserInputsConfig.KeyMoveForward, GetOnInputAction(Vector3.forward), (e) => _inputEvents.Add(e));
            _im.StartListeningForKey(UserInputsConfig.KeyMoveBackward, GetOnInputAction(Vector3.back), (e) => _inputEvents.Add(e));
            _im.StartListeningForKey(UserInputsConfig.KeyMoveLeftward, GetOnInputAction(Vector3.left), (e) => _inputEvents.Add(e));
            _im.StartListeningForKey(UserInputsConfig.KeyMoveRightward, GetOnInputAction(Vector3.right), (e) => _inputEvents.Add(e));
        }
        Action GetOnInputAction(Vector3 direction) {
            return () => {
                _targetPos += direction * WalkSpeed * (InputManager.GetKey(UserInputsConfig.KeyMoveSprintward) ? RunSpeedModifier : 1);
                PositionChanged?.Invoke(_targetPos);
            };
        }


        void FixedUpdate() {
            transform.position = Vector3.Lerp(transform.position, _targetPos, Lag);
        }
    }
}