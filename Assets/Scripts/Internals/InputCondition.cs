using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace OmniGlyph.Internals {
    public class InputCondition : IICondition {
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
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            InputCondition c = (InputCondition)obj;
            return keys.SequenceEqual(c.keys) && inputType == c.inputType;
        }
        public override int GetHashCode() {
            int hash = 17;
            foreach (KeyCode key in keys) {
                hash = hash ^ 31 + key.GetHashCode();
            }
            hash = hash ^ 31 + inputType.GetHashCode();
            return hash;
        }
    }
}
