using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace OmniGlyph.Internals.Events {
    public record InputCondition : ICondition {
        public KeyCode[] keys;
        public InputTypes inputType;
        public InputCondition(KeyCode[] keys, InputTypes inputType) {
            this.keys = keys;
            this.inputType = inputType;
        }
        public bool Is() {
            switch (this.inputType) {
                case InputTypes.KeyDown:
                    return keys.All(key => Input.GetKeyDown(key));
                case InputTypes.KeyUp:
                    return keys.All(key => Input.GetKeyUp(key));
                case InputTypes.KeyHeld:
                    return keys.All(key => Input.GetKey(key));
                default:
                    return false;
            }
        }
        public override string ToString() {
            return $"InputCondition: {string.Join("+", keys)}, {inputType}";
        }
    }
}
