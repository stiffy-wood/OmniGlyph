using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGlyph.Control {
    [CreateAssetMenu(fileName = "NewContextKeyBinds", menuName = "Input/ContextKeyBinds")]
    public class ContextKeyBinds : ScriptableObject {
        [SerializeField]
        KeyBind[] _keyBinds;
        public Dictionary<string, KeyCode> Load() {
            Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>();
            foreach (KeyBind keyBind in _keyBinds) {
                keyBinds.Add(keyBind.name, keyBind.key);
            }
            return keyBinds;
        }
    }
}
