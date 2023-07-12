using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Internals;
using OmniGlyph.Internals.Debugging;
using UnityEngine;

namespace OmniGlyph.Control {
    public class InputManager : OmniMonoComponent {
        private Dictionary<ControlActions, KeyCode> _currentKeyBinds;

        public override void Init(GameContext context) {
            base.Init(context);
            Context.GameStateChanged += OnGameStateChange;
            _currentKeyBinds = Context.StateContext.KeyBinds.Load();
        }
        private void OnGameStateChange(GameStates newState) {
            _currentKeyBinds = Context.StateContext.KeyBinds.Load();
        }
        private float InputToFloat(KeyCode positiveKey, KeyCode negativeKey) {
            float value = 0;
            if (Input.GetKey(positiveKey)) {
                value += 1;
            }
            if (Input.GetKey(negativeKey)) {
                value -= 1;
            }
            return value;
        }
        private Maybe<KeyCode> GetKeyBind(ControlActions keyBind) {
            return _currentKeyBinds.TryGetValue(keyBind, out KeyCode keyCode) ? Maybe<KeyCode>.Some(keyCode) : Maybe<KeyCode>.None();
        }
        private Maybe<KeyCode[]> GetKeyBinds(params ControlActions[] keyBinds) {
            List<KeyCode> keyCodes = new List<KeyCode>();
            foreach (ControlActions keyBind in keyBinds) {
                Maybe<KeyCode> maybeKeyCode = GetKeyBind(keyBind);
                if (maybeKeyCode.HasValue) {
                    keyCodes.Add(maybeKeyCode.Value);
                }
            }
            return keyCodes.Count == keyBinds.Length ? Maybe<KeyCode[]>.Some(keyCodes.ToArray()) : Maybe<KeyCode[]>.None();
        }
        public Maybe<Vector3> GetPlayerMovement() {
            Maybe<KeyCode[]> keyCodes = GetKeyBinds(ControlActions.MoveForward, ControlActions.MoveBackward, ControlActions.MoveLeft, ControlActions.MoveRight);
            if (!keyCodes.HasValue) {
                return Maybe<Vector3>.None();
            }
            return Maybe<Vector3>.Some(
                new Vector3(InputToFloat(keyCodes.Value[3], keyCodes.Value[2]),
                            0,
                            InputToFloat(keyCodes.Value[0], keyCodes.Value[1]))
                );
        }
        public Maybe<bool> IsPlayerSprinting() {
            Maybe<KeyCode> keyCode = GetKeyBind(ControlActions.Run);
            return keyCode.HasValue ? Maybe<bool>.Some(Input.GetKey(keyCode.Value)) : Maybe<bool>.None();
        }
        public Maybe<bool> IsPlayerInteracting() {
            Maybe<KeyCode> keyCode = GetKeyBind(ControlActions.Interact);
            return keyCode.HasValue ? Maybe<bool>.Some(Input.GetKey(keyCode.Value)) : Maybe<bool>.None();
        }
        public Maybe<bool> IsTestButtonPressed() {
            Maybe<KeyCode> keyCode = GetKeyBind(ControlActions.Test);
            return keyCode.HasValue ? Maybe<bool>.Some(Input.GetKey(keyCode.Value)) : Maybe<bool>.None();
        }
        public bool GetMouseButtonUp(MouseButtons button) {
            return Input.GetMouseButtonUp((int)button);
        }
        public bool GetMouseButtonDown(MouseButtons button) {
            return Input.GetMouseButtonDown((int)button);
        }
        public bool GetMouseButton(MouseButtons button) {
            return Input.GetMouseButton((int)button);
        }
    }
}
