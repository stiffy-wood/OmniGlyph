using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Control {
    [CreateAssetMenu(fileName = "NewContextKeyBinds", menuName = "Input/ContextKeyBinds")]
    public class ContextKeyBinds : ScriptableObject {
        [SerializeField]
        KeyBind[] _keyBinds;
        public Dictionary<ControlActions, KeyCode> Load() {
            Dictionary<ControlActions, KeyCode> keyBinds = new Dictionary<ControlActions, KeyCode>();
            foreach (KeyBind keyBind in _keyBinds) {
                keyBinds.Add(keyBind.name, keyBind.key);
            }
            return keyBinds;
        }
    }
}
